using Api.Database;
using Api.Entities;
using Api.Extensions;
using Api.Features.InventoryMaintenance.Command;
using Api.Features.PurchaseOrderMaintenance;
using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace Api.Features.InvoiceFeature.Command;

public record InvoiceLineItemReq(
    int ItemId,
    int Quantity,
    decimal Discount);

public record CreateInvoiceCommand(
    int CustomerId,
    PaymentMethod PaymentMethod, // cash, on-account
    List<InvoiceLineItemReq> LineItems,

    // Journal
    int DebitTo,
    int CreditTo
) : IRequest<Result<Invoice>>;

internal sealed class CreateInvoiceCommandHandler 
    : IRequestHandler<CreateInvoiceCommand, Result<Invoice>>
{
    private static readonly int SALES_JOURNAL_PK = 6;
    private static readonly int RECEIPT_JOURNAL_PK = 4;
    private static readonly Dictionary<PaymentMethod, int> PaymentMethodJournalTypeMap = new()
    {
        { PaymentMethod.Cash, RECEIPT_JOURNAL_PK },
        { PaymentMethod.OnAccount, SALES_JOURNAL_PK }
    };

    private readonly IMediator _mediator;
    private readonly ApplicationDbContext _dbContext;

    public CreateInvoiceCommandHandler(IMediator mediator, ApplicationDbContext dbContext)
    {
        _mediator = mediator;
        _dbContext = dbContext;
    }

    public async Task<Result<Invoice>> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        var validation = new CreateInvoiceCommandValidator().Validate(request);

        var customer = await _dbContext.Customers.FindAsync([request.CustomerId], cancellationToken);
        var items = await ValidateLineItems(validation, request, cancellationToken);
        var debitTo = await _dbContext.Accounts.FindAsync([request.DebitTo], cancellationToken);
        var creditTo = await _dbContext.Accounts.FindAsync([request.CreditTo], cancellationToken);

        validation.AddErrorIfNull(customer, nameof(request.CustomerId), AppErrorCode.EntityNotFound);
        validation.AddErrorIfNull(debitTo, nameof(request.DebitTo), AppErrorCode.EntityNotFound);
        validation.AddErrorIfNull(creditTo, nameof(request.CreditTo), AppErrorCode.EntityNotFound);

        if (!validation.IsValid)
        {
            return Result.Invalid(validation.AsErrors());
        }

        // TODO: Check stock

        // Entity line item
        List<InvoiceLineItem> lineItems = request.LineItems
            .Select(requestLine => InvoiceLineItem.FromItem(
                requestLine.Quantity,
                requestLine.Discount,
                items.Single(e => e.Id == requestLine.ItemId)))
            .ToList();

        decimal sales = lineItems.Select(e => e.LineTotal).Sum();
        decimal vatableAmount = Math.Round(sales / 1.12m, 2);
        decimal vat = Math.Round(vatableAmount * 0.12m, 2);

        var invoice = new Invoice
        {
            PaymentMethod = request.PaymentMethod,
            Customer = customer!,
            LineItems = lineItems,
            VatableAmount = vatableAmount,
            VatAmount = vat,
            NetAmount = vatableAmount + vat,
            InstalmentBalance = (request.PaymentMethod == PaymentMethod.OnAccount ? vatableAmount + vat : null),
            JournalEntry = null!
        };

        if (invoice.NetAmount != sales)
        {
            throw new InvalidOperationException($"sales({sales}) <> netamount({invoice.NetAmount}). Check rounding errors");
        }

        // Journal posting
        // Debit : 1 line  (Cash, Receivable, etc..)
        // Credit: 1 line per line item

        var journalType = await _dbContext.JournalTypes.FindAsync(
            [ PaymentMethodJournalTypeMap[request.PaymentMethod] ],
            cancellationToken
        );
        var debitLine = new JournalLine
        {
            Description = request.PaymentMethod.ToString(),
            Account = debitTo!,
            Debit = invoice.NetAmount,
        };
        var creditLines = lineItems.Select(lineItem => new JournalLine
        {
            Description = lineItem.ItemSnapshot.Name,
            Account = creditTo!,
            Credit = lineItem.LineTotal,
            ReferenceNumber1 = lineItem.ItemSnapshot.Code,
        });
        var entry = new JournalEntry
        {
            JournalType = journalType!,
            Description = string.Empty,
            Lines = [debitLine, .. creditLines]
        };

        invoice.JournalEntry = entry;

        using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        
        try
        {
            await _dbContext.Invoices.AddAsync(invoice, cancellationToken);
            await _dbContext.JournalEntries.AddAsync(entry, cancellationToken);

            // Decrease stock
            foreach (var i in lineItems)
            {
                await _mediator.Send(new ModifyStockCommand(i.OriginalItem.Id, i.Quantity * -1), cancellationToken);
            }

            await _dbContext.SaveChangesAsync(cancellationToken);

            await transaction.CommitAsync(cancellationToken);
            return Result<Invoice>.Success(invoice);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<List<Item>> ValidateLineItems(ValidationResult validation, CreateInvoiceCommand request, CancellationToken cancellationToken)
    {
        var itemIds = request.LineItems.Select(e => e.ItemId).Distinct().ToArray();
        var items = await _dbContext.Items
            .Include(e => e.Uom)
            .Where(e => itemIds.Contains(e.Id))
            .ToListAsync(cancellationToken);

        if (items.Count < itemIds.Length)
        {
            validation.AddError(nameof(request.LineItems), AppErrorCode.LineItemElementNotFound);
        }

        // Check invalid line items
        foreach (var item in items)
        {
            if (item.UnitPrice <= 0)
            {
                validation.AddError(nameof(request.LineItems), AppErrorCode.LineItemInvalidUnitPrice);
                break;
            }
        }

        return items;
    }
}

public class InvoiceLineItemReqValidator : AbstractValidator<InvoiceLineItemReq>
{
    public InvoiceLineItemReqValidator()
    {
        RuleFor(e => e.ItemId).GreaterThanOrEqualTo(0);
        RuleFor(e => e.Quantity).GreaterThan(0);
        RuleFor(e => e.Discount).GreaterThanOrEqualTo(0);
    }
}

public class CreateInvoiceCommandValidator : AbstractValidator<CreateInvoiceCommand>
{
    public CreateInvoiceCommandValidator()
    {
        RuleFor(e => e.PaymentMethod).IsInEnum();
        RuleFor(e => e.CustomerId).GreaterThanOrEqualTo(0);
        RuleFor(e => e.LineItems).NotEmpty();
        RuleForEach(e => e.LineItems).SetValidator(new InvoiceLineItemReqValidator());
        RuleFor(e => e.LineItems)
            .Must(e =>
            {
                // Must have distinct line item IDs
                var ids = e.Select(li => li.ItemId).ToList();
                return new HashSet<int>(ids).Count == ids.Count; // Distinct
            })
            .WithErrorCode(ErrorCodes.E_NON_DISTINCT_LINE_ITEMS)
            .WithMessage("Line items must be unique. Combine similar items in a single line");
        RuleFor(e => e.DebitTo).GreaterThanOrEqualTo(0);
        RuleFor(e => e.CreditTo).GreaterThanOrEqualTo(0);
        RuleFor(e => e.DebitTo)
            .NotEqual(e => e.CreditTo)
            .WithMessage(AppErrorCode.IdenticalDebitAndCreditAccount.Code);
    }
}