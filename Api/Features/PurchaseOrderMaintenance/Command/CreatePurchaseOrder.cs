using Api.Database;
using Api.Entities;
using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace Api.Features.PurchaseOrderMaintenance.Command;

public record PurchaseOrderLineItem(
    int ItemId,
    int Quantity,
    decimal Discount // In amount, not percentage
);

public record CreatePurchaseOrderCommand(
    //int? RRNumber,
    string RequisitionNumber,
    int SupplierId,
    DateOnly DeliveryDate,
    int ProjectId,
    string Description,
    List<PurchaseOrderLineItem> LineItems,
    int DebitTo,
    int CreditTo
) : IRequest<Result<PurchaseOrder>>;

public class PurchaseOrderCommandValidator : AbstractValidator<CreatePurchaseOrderCommand>
{
    public PurchaseOrderCommandValidator()
    {
        // Must be integral
        RuleFor(e => e.RequisitionNumber)
            .NotEmpty()
            .Must(e => int.TryParse(e, out int _))
            .WithMessage("Must be integral");
        RuleFor(e => e.SupplierId).GreaterThanOrEqualTo(0);
        RuleFor(e => e.DeliveryDate).NotEmpty();
        RuleFor(e => e.ProjectId).GreaterThanOrEqualTo(0);
        RuleFor(e => e.Description).MinimumLength(3);
        RuleFor(e => e.DebitTo).GreaterThanOrEqualTo(0);
        RuleFor(e => e.CreditTo).GreaterThanOrEqualTo(0);
        RuleFor(e => e.DebitTo)
            .NotEqual(e => e.CreditTo)
            .WithMessage(ErrorCodes.E_IDENTICAL_DEBIT_CREDIT_TARGET);
        RuleFor(e => e.LineItems).NotEmpty();
        RuleForEach(e => e.LineItems).SetValidator(new PurchaseOrderLineItemValidator());
        RuleFor(e => e.LineItems)
            .Must(DistinctElements)
            .WithErrorCode(ErrorCodes.E_NON_DISTINCT_LINE_ITEMS)
            .WithMessage("Line items must be unique. Combine similar items in a single line");
    }

    private static bool DistinctElements(List<PurchaseOrderLineItem> items)
    {
        var ids = items.Select(e => e.ItemId).ToList();
        return new HashSet<int>(ids).Count == ids.Count;
    }
}

public class PurchaseOrderLineItemValidator : AbstractValidator<PurchaseOrderLineItem>
{
    public PurchaseOrderLineItemValidator()
    {
        RuleFor(e => e.ItemId).GreaterThanOrEqualTo(0);
        RuleFor(e => e.Quantity).GreaterThan(0);
        RuleFor(e => e.Discount).GreaterThanOrEqualTo(0);
    }
}

internal sealed class CreatePurchaseOrderCommandHandler
    : IRequestHandler<CreatePurchaseOrderCommand, Result<PurchaseOrder>>
{
    private readonly ApplicationDbContext _dbContext;

    public CreatePurchaseOrderCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<PurchaseOrder>> Handle(CreatePurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        var validation = new PurchaseOrderCommandValidator().Validate(request);

        Supplier? supplier = await _dbContext.Suppliers.FindAsync([ request.SupplierId ], cancellationToken);
        if (supplier == null)
        {
            validation.Errors.Add(
                new ValidationFailure(
                    nameof(request.SupplierId),
                    ErrorCodes.E_SUPPLIER_NOT_FOUND)
            );
        }

        Project? project = await _dbContext.Projects.FindAsync([request.ProjectId], cancellationToken);
        if (project == null)
        {
            validation.Errors.Add(
                new ValidationFailure(
                    nameof(request.ProjectId),
                    ErrorCodes.E_PROJECT_NOT_FOUND)
            );
        }

        if (! await _dbContext.Accounts.AnyAsync(e => e.Id == request.DebitTo, cancellationToken))
        {
            validation.Errors.Add(
                new ValidationFailure(
                    nameof(request.DebitTo),
                    ErrorCodes.E_INVALID_ACCOUNT)
            );
        }

        if (! await _dbContext.Accounts.AnyAsync(e => e.Id == request.CreditTo, cancellationToken))
        {
            validation.Errors.Add(
                new ValidationFailure(
                    nameof(request.CreditTo),
                    ErrorCodes.E_INVALID_ACCOUNT)
            );
        }

        var ids = request.LineItems.Select(e => e.ItemId).ToArray();
        var items = await _dbContext.Items
            //.AsNoTracking()
            .Include(e => e.Uom)
            .Where(e => ids.Contains(e.Id))
            .ToListAsync(cancellationToken);

        if (items.Count < ids.Length)
        {
            validation.Errors.Add(
                new ValidationFailure(
                    nameof(request.LineItems),
                    ErrorCodes.E_ITEM_NOT_FOUND)
            );
        }

        if (!validation.IsValid)
        {
            return Result.Invalid(validation.AsErrors());
        }

        // TODO: Check for zero cost items

        var lineItems = request.LineItems
            .Select(requestLine => LineItem.FromItem(
                requestLine.Quantity,
                requestLine.Discount,
                items.Single(e => e.Id == requestLine.ItemId)))
            .ToList();
        
        decimal sales = lineItems.Select(e => e.LineTotal).Sum();
        decimal vatableAmount = Math.Round(sales / 1.12m, 2);
        decimal vatAmount = Math.Round(vatableAmount * 0.12m, 2);

        var po = new PurchaseOrder
        {
            RequisitionNumber = request.RequisitionNumber,
            Description = request.Description,
            Project = project!,
            Supplier = supplier!,
            DeliveryDate = request.DeliveryDate,
            CreatedAtDate = DateOnly.FromDateTime(DateTime.Now),
            LineItems = lineItems,
            VatableAmount = vatableAmount,
            VatAmount = vatAmount,
            //Discounted = request.Discou
            NetAmount = vatableAmount + vatAmount,
            DebitTo = request.DebitTo,
            CreditTo = request.CreditTo
        };

        if (po.NetAmount != sales)
        {
            throw new InvalidOperationException($"sales({sales}) <> netamount({po.NetAmount}). Check rounding errors");
        }

        await _dbContext.PurchaseOrders.AddAsync(po, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result<PurchaseOrder>.Success(po);
    }
}