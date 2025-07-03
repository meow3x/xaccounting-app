using Api.Database;
using Api.Entities;
using Api.Extensions;
using Api.Features.Journal;
using Api.Features.PurchaseOrderMaintenance;
using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Threading;

namespace Api.Features.Collections.Command;

public record PostColPaymentCommand(
    int InvoiceNumber,
    List<JournalEntryLine> Lines,
    string ReferenceNumber
) : IRequest<Result<CollectionPayment>>
{
    [JsonIgnore]
    public decimal Balance =>
       (Lines?.Where(e => e.Debit.HasValue).Sum(e => e.Debit) ?? 0)
       - (Lines?.Where(e => e.Credit.HasValue).Sum(e => e.Credit) ?? 0);
}

public class ColPaymentCommandValidator : AbstractValidator<PostColPaymentCommand>
{
    public ColPaymentCommandValidator()
    {
        RuleFor(e => e.InvoiceNumber).GreaterThan(0);
        //RuleFor(e => e.Amount).GreaterThan(0);
        RuleFor(e => e.ReferenceNumber).MinimumLength(3);
        RuleFor(e => e.Balance).Equal(0);
        RuleFor(e => e.Lines.Count).GreaterThanOrEqualTo(2);
        RuleForEach(e => e.Lines).SetValidator(new JournalEntryLineValidator());
    }
}

internal sealed class PostColPaymentCommandHandler
    : IRequestHandler<PostColPaymentCommand, Result<CollectionPayment>>
{
    private const int RECEIPTS_JOURNAL_TYPE_PK = 4;
    private readonly ApplicationDbContext _dbContext;

    public PostColPaymentCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<CollectionPayment>> Handle(PostColPaymentCommand request, CancellationToken cancellationToken)
    {
        var validation = new ColPaymentCommandValidator().Validate(request);
        var invoice = await _dbContext.Invoices.SingleOrDefaultAsync(e => e.Number == request.InvoiceNumber, cancellationToken);

        validation.AddErrorIfNull(invoice, nameof(request.InvoiceNumber), AppErrorCode.EntityNotFound);

        if (invoice?.PaymentMethod != PaymentMethod.OnAccount)
        {
            validation.AddError(nameof(request.InvoiceNumber), AppErrorCode.NotAnInstalment);
        }

        if (!validation.IsValid)
        {
            return Result<CollectionPayment>.Invalid(validation.AsErrors());
        }

        var je = await BuildJournalEntry(request, cancellationToken);

        var payment = new CollectionPayment
        {
            ReferenceNumber = request.ReferenceNumber,
            Amount = je.Lines.Where(e => e.Credit.HasValue).Sum(e => e.Credit)!.Value,
            JournalEntry = je
        };

        // Alter invoice balance
        invoice!.InstalmentBalance -= payment.Amount;

        invoice.Payments.Add(payment);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return payment;
    }

    private async Task<JournalEntry> BuildJournalEntry(PostColPaymentCommand request, CancellationToken cancellationToken)
    {
        decimal total = 0.0m;
        List<JournalLine> lines = [];

        foreach (var jl in request.Lines)
        {
            CostCenter? costCenter = null;

            if (jl.CostCenterId.HasValue)
            {
                costCenter = await _dbContext.CostCenters.FindAsync([jl.CostCenterId], cancellationToken)
                    ?? throw new ArgumentOutOfRangeException(nameof(request), ErrorCodes.E_COST_CENTER_NOT_FOUND);
            }

            var account = await _dbContext.Accounts.FindAsync([jl.AccountId], cancellationToken)
                ?? throw new ArgumentOutOfRangeException(nameof(request), AppErrorCode.EntityNotFound.ToString());

            lines.Add(new JournalLine
            {
                ReferenceNumber1 = jl.ReferenceNumber,
                Description = jl.Description,
                Account = account,
                Debit = jl.Debit,
                Credit = jl.Credit,
                CostCenter = costCenter
            });

            if (jl.Credit.HasValue) total += jl.Credit.Value;
        }

        var journalType = await _dbContext.JournalTypes.FindAsync([RECEIPTS_JOURNAL_TYPE_PK], cancellationToken);

        return new JournalEntry
        {
            JournalType = journalType!,
            Description = string.Empty,
            Lines = lines
        };
    }
}