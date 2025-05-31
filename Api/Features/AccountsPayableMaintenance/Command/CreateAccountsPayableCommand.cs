using Api.Database;
using Api.Entities;
using Api.Features.Journal;
using Api.Features.PurchaseOrderMaintenance;
using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;
using MediatR;
using System.Text.Json.Serialization;

namespace Api.Features.AccountsPayableMaintenance.Command;

public record CreateAccountsPayableCommand(
    int SupplierId, // FIXME: This can be per journal line!
    string ReferenceNumber, // To be printed on voucher form
    DateOnly? DueDate, // Autocompute from terms?
    List<JournalEntryLine> Lines
) : IRequest<Result<AccountsPayable>>
{
    [JsonIgnore]
    public decimal Balance =>
       (Lines?.Where(e => e.Debit.HasValue).Sum(e => e.Debit) ?? 0)
       - (Lines?.Where(e => e.Credit.HasValue).Sum(e => e.Credit) ?? 0);
}


public class CreateAccountsPayableCommandValidator : AbstractValidator<CreateAccountsPayableCommand>
{
    public CreateAccountsPayableCommandValidator()
    {
        RuleFor(e => e.SupplierId).NotEmpty();
        RuleFor(e => e.ReferenceNumber).NotEmpty();
        RuleFor(e => e.Balance).Equal(0);
        RuleFor(e => e.DueDate).NotEmpty().When(e => e.DueDate != null);
        RuleFor(e => e.Lines.Count).GreaterThanOrEqualTo(2);
        RuleForEach(e => e.Lines).SetValidator(new JournalEntryLineValidator());
        RuleForEach(e => e.Lines).ChildRules(jl =>
        {
            jl.RuleFor(e => e.CostCenterId).NotEmpty(); // Cost center is required for AP
        });
    }
}

public class CreateAccountsPayableCommandHandler
    : IRequestHandler<CreateAccountsPayableCommand, Result<AccountsPayable>>
{
    private readonly ApplicationDbContext _dbContext;
    private const int PAYABLES_JOURNAL_PK = 3;

    public CreateAccountsPayableCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<AccountsPayable>> Handle(CreateAccountsPayableCommand request, CancellationToken cancellationToken)
    {
        var validation = new CreateAccountsPayableCommandValidator().Validate(request);

        var supplier = await _dbContext.Suppliers.FindAsync([request.SupplierId], cancellationToken);
        if (supplier == null)
        {
            validation.Errors.Add(new(nameof(request.SupplierId), ErrorCodes.E_SUPPLIER_NOT_FOUND));
        }

        if (!validation.IsValid)
        {
            return Result<AccountsPayable>.Invalid(validation.AsErrors());
        }

        decimal total = 0.0m;
        List<JournalLine> lines = [];

        foreach(var jl in request.Lines)
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
                Description = jl.Description,
                Account = account,
                Debit = jl.Debit,
                Credit = jl.Credit,
                CostCenter = costCenter
            });

            if (jl.Debit.HasValue) total += jl.Debit.Value;
        }

        var ap = new AccountsPayable
        {
            Supplier = supplier!,
            ReferenceNumber = request.ReferenceNumber,
            DueDate = request.DueDate,
            JournalEntry = new JournalEntry
            {
                JournalType = (await _dbContext.JournalTypes.FindAsync([PAYABLES_JOURNAL_PK], cancellationToken))!,
                Description = "",
                Lines = lines
            },
            TotalAmount = total,
            Balance = total
        };

        await _dbContext.AccountsPayable.AddAsync(ap, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return ap;
    }
}