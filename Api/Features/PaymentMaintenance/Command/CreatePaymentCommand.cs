using Api.Database;
using Api.Entities;
using Api.Features.AccountsPayableMaintenance.Command;
using Api.Features.Journal;
using Api.Features.PurchaseOrderMaintenance;
using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;
using MediatR;
using System.Text.Json.Serialization;

namespace Api.Features.PaymentMaintenance.Command;

public record CreatePaymentCommand(
    int PayeeId,
    bool IsCheque,
    string ReferenceNumber,
    List<JournalEntryLine> Lines
) : IRequest<Result<Payment>>
{
    [JsonIgnore]
    public decimal Balance =>
       (Lines?.Where(e => e.Debit.HasValue).Sum(e => e.Debit) ?? 0)
       - (Lines?.Where(e => e.Credit.HasValue).Sum(e => e.Credit) ?? 0);
}


public class CreatePaymentCommandValidator : AbstractValidator<CreatePaymentCommand>
{
    public CreatePaymentCommandValidator()
    {
        RuleFor(e => e.PayeeId).NotEmpty();
        RuleFor(e => e.IsCheque).NotNull();
        RuleFor(e => e.ReferenceNumber).NotEmpty();
        RuleFor(e => e.ReferenceNumber)
            .Must(e => int.TryParse(e, out _))
            .When(e => e.IsCheque)
            .WithMessage("'Reference Number' must be integral and set to cheque number if payment is cheque"); // Must be integral if cheque payment
        RuleFor(e => e.Balance).Equal(0);
        RuleFor(e => e.Lines.Count).GreaterThanOrEqualTo(2);
        RuleForEach(e => e.Lines).SetValidator(new JournalEntryLineValidator());
    }
}

internal class CreateDisbursementCommandHandler
    : IRequestHandler<CreatePaymentCommand, Result<Payment>>
{
    private const int DISBURSEMENT_JOURNAL_TYPE_PK = 2;
    private readonly ApplicationDbContext _dbContext;

    public CreateDisbursementCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Payment>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        var validation = new CreatePaymentCommandValidator().Validate(request);

        var payee = await _dbContext.Suppliers.FindAsync([request.PayeeId], cancellationToken);
        if (payee == null)
        {
            validation.Errors.Add(new(nameof(request.PayeeId), ErrorCodes.E_SUPPLIER_NOT_FOUND));
        }

        if (!validation.IsValid)
        {
            return Result<Payment>.Invalid(validation.AsErrors());
        }

        // Map journal lines
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
                ?? throw new ArgumentOutOfRangeException(nameof(request), ErrorCodes.E_INVALID_ACCOUNT);

            lines.Add(new JournalLine
            {
                ReferenceNumber1 = jl.ReferenceNumber,
                Description = jl.Description,
                Account = account,
                Debit = jl.Debit,
                Credit = jl.Credit,
                CostCenter = costCenter
            });
        }

        var payment = new Payment
        {
            ReferenceNumber = request.ReferenceNumber,
            Payee = payee!,
            IsCheque = request.IsCheque,
            ChequeStatus = request.IsCheque ? ChequeStatus.Pending : null,
            JournalEntry = new JournalEntry
            {
                JournalType = (await _dbContext.JournalTypes.FindAsync([DISBURSEMENT_JOURNAL_TYPE_PK], cancellationToken))!,
                Description = "",
                Lines = lines
            },
        };
           
        await _dbContext.Payments.AddAsync(payment, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result<Payment>.Success(payment);
    }
}