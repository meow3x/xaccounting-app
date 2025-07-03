using Api.Database;
using Api.Entities;
using Api.Features.Journal;
using Ardalis.Result;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using NodaTime;
using System.Text.Json.Serialization;
using System.Threading;

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
    
    public ReceivePurchaseOrderCommandHandler(ApplicationDbContext dbContext, IMediator mediator)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<PurchaseOrder>> Handle(ReceivePurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        // 1. open transaction
        // 2. Post debit/credit journal entry
        // 3. mark as closed
        var (result, po, debitAccount, creditAccount) = await Validate(request, cancellationToken);
        
        if (!result.IsSuccess)
        {
            return result;
        }

        JournalType? purchaseJournal = await _dbContext.JournalTypes.FindAsync([PURCHASE_JOURNAL_PK], cancellationToken);
        
        ArgumentNullException.ThrowIfNull(purchaseJournal, nameof(purchaseJournal));
        
        // One journal entry line per item
        var debitLines = po!.LineItems.Select(lineItem => new JournalLine
        {
            Description = lineItem.ItemSnapshot.Name,
            Account = debitAccount!,
            Debit = lineItem.LineTotal,
            ReferenceNumber1 = po.Number.ToString()
        });
        var creditLine = new JournalLine
        {
            Description = "Purchased Merchandise on Account",
            Account = creditAccount!,
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
        po.ClosedAt = SystemClock.Instance.GetCurrentInstant();

        // Increase stock
        // TODO: Move this to dedicated mediator

        foreach (var lineItem in po!.LineItems)
        {
            var inventory = await _dbContext.Inventory
                .Include(e => e.Logs)
                .SingleOrDefaultAsync(
                    e => e.Item.Id == lineItem.OriginalItem.Id,
                    cancellationToken
                );
            if (inventory == null)
            {
                // FIXME: Possible race condition (two POs referring to the same item is submitted, but no inventory record yet)
                inventory = new Inventory
                {
                    Item = (await _dbContext.Items.FindAsync([lineItem.OriginalItem.Id], cancellationToken))!,
                    Stock = 0 //lineItem.Quantity
                };

                await _dbContext.Inventory.AddAsync(inventory, cancellationToken);
            }

            // TODO: Variable line item quantity receiving
            inventory.Logs.Add(
                new InventoryLog
                {
                    Inventory = inventory,
                    LineItem = lineItem,
                    StockBefore = inventory.Stock,
                    Quantity = lineItem.Quantity, 
                    StockAfter = inventory.Stock + lineItem.Quantity // Fixme: race condition
                });
            inventory.Stock += lineItem.Quantity; // FIXME: race condtition
            _dbContext.Entry(inventory).State = EntityState.Modified; // Make sure modifications are detected, in case this is a new inventory entry
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        return Result<PurchaseOrder>.Success(po);
    }

    private async Task<(
        Result result,
        PurchaseOrder? po,
        Account? debit,
        Account? credit)>
    Validate(ReceivePurchaseOrderCommand request, CancellationToken cancellationToken)
    {
        PurchaseOrder? po = await _dbContext.PurchaseOrders
            .Include(po => po.LineItems)
            .ThenInclude(li => li.OriginalItem)
            .SingleOrDefaultAsync(po => po.Id == request.Id, cancellationToken);

        if (po == null)
        {
            return ErrorCode(nameof(request.Id), ErrorCodes.E_ORDER_NOT_FOUND);
        }

        if (po.Status != OrderStatus.Open)
        {
            return ErrorCode(nameof(request.Id), ErrorCodes.E_ORDER_NOT_OPEN);
        }

        if (request.DebitTo == request.CreditTo)
        {
            return ErrorCode(
                nameof(request.DebitTo) + ", " + nameof(request.CreditTo),
                ErrorCodes.E_IDENTICAL_DEBIT_CREDIT_TARGET
            );
        }

        var debitAccount = await _dbContext.Accounts.FindAsync([request.DebitTo], cancellationToken);
        if (debitAccount == null)
        {
            return ErrorCode(nameof(request.DebitTo), ErrorCodes.E_INVALID_ACCOUNT);
        }

        var creditAccount = await _dbContext.Accounts.FindAsync([request.CreditTo], cancellationToken);
        if (creditAccount == null)
        {
            return ErrorCode(nameof(request.CreditTo), ErrorCodes.E_INVALID_ACCOUNT);
        }

        return (Result.Success(), po, debitAccount, creditAccount);
    }

    private static (
        Result result,
        PurchaseOrder? po,
        Account? debit,
        Account? credit)
    ErrorCode(string identifier, string errorMessage)
    {
        return (
            Result.Invalid(new ValidationError(identifier, errorMessage)),
            null,
            null,
            null
        );
    }
}

