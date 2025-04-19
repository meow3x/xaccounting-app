using Api.Database;
using Api.Entities;
using Api.Features.Journal;
using Ardalis.Result;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Api.Features.PurchaseOrderMaintenance.Command;

public record ReceivePurchaseOrderCommand(
    //string RRNumber // TODO
    [property: JsonIgnore] int Id,
    int DebitTo,
    int CreditTo
) : IRequest<Result<PurchaseOrder>>;

//public class ReceiveOrderCommandValidator : AbstractValidator<ReceivePurchaseOrderCommand>
//{
//    public ReceiveOrderCommandValidator()
//    {
//        RuleFor(e => e.OrderNumber).GreaterThanOrEqualTo(0);
//        RuleFor(e => e.Quantity).GreaterThan(0);
//        RuleFor(e => e.Discount).GreaterThanOrEqualTo(0);
//    }
//}
internal sealed class ReceivePurchaseOrderCommandHandler
    : IRequestHandler<ReceivePurchaseOrderCommand, Result<PurchaseOrder>>
{
    private const int PURCHASE_JOURNAL_PK = 5;
    private readonly ApplicationDbContext _dbContext;
    private readonly IMediator _mediator;
    
    public ReceivePurchaseOrderCommandHandler(ApplicationDbContext dbContext, IMediator mediator)
    {
        _dbContext = dbContext;
        _mediator = mediator;
    }

    public async Task<Result<PurchaseOrder>> Handle(ReceivePurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        // 1. open transaction
        // 2. Post debit/credit journal entry
        // 3. mark as closed
        var po = await _dbContext.PurchaseOrders
            .Include(po => po.LineItems)
            .SingleOrDefaultAsync(po => po.Id == request.Id, cancellationToken);

        if (po == null)
        {
            return Result.Invalid(new ValidationError(
                nameof(request.Id),
                ErrorCodes.E_ORDER_NOT_FOUND));
        }

        if (po.Status != OrderStatus.Open)
        {
            return Result.Invalid(new ValidationError(
                nameof(request.Id),
                ErrorCodes.E_ORDER_NOT_OPEN));
        }

        if (request.DebitTo == request.CreditTo)
        {
            return Result.Invalid(new ValidationError(
                nameof(request.DebitTo) + ", " + nameof(request.CreditTo),
                ErrorCodes.E_IDENTICAL_DEBIT_CREDIT_TARGET));
        }

        var debitAccount = await _dbContext.Accounts.FindAsync([request.DebitTo], cancellationToken);
        if (debitAccount == null)
        {
            return Result.Invalid(new ValidationError(
               nameof(request.Id),
               ErrorCodes.E_INVALID_ACCOUNT));
        }

        var creditAccount = await _dbContext.Accounts.FindAsync([request.CreditTo], cancellationToken);
        if (creditAccount == null)
        {
            return Result.Invalid(new ValidationError(
               nameof(request.Id),
               ErrorCodes.E_INVALID_ACCOUNT));
        }

        var purchaseJournal = await _dbContext.JournalTypes.FindAsync([PURCHASE_JOURNAL_PK], cancellationToken);

        // One journal entry line per item
        var debitLines = po.LineItems
            .Select(lineItem => new JournalLine
            {
                Description = lineItem.ItemSnapshot.Name,
                Account = debitAccount,
                Debit = lineItem.LineTotal,
                ReferenceNumber1 = po.Number.ToString()
            });
        var creditLine = new JournalLine
        {
            Description = "Purchased Merchandise on Account",
            Account = creditAccount,
            Credit = po.NetAmount,
            ReferenceNumber1 = po.Number.ToString()
        };
        var entry = new JournalEntry
        {
            JournalType = purchaseJournal!,
            Description = string.Empty,
            Lines = [.. debitLines, creditLine],
            ReferenceNumber1 = po.Number.ToString()
        };

        // Post journal entry
        await _dbContext.JournalEntries.AddAsync(entry, cancellationToken);

        // Mark as closed
        // Update target debit/credot
        po.DebitTo = request.DebitTo;
        po.CreditTo = request.CreditTo;
        po.Status = OrderStatus.Closed;
        po.ClosedAt = DateTime.Now;

        // Increase stock
        // ...

        await _dbContext.SaveChangesAsync(cancellationToken);
        return Result<PurchaseOrder>.Success(po);
    }
}

