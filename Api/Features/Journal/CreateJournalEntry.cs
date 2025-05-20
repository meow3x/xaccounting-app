using FluentValidation;

namespace Api.Features.Journal;

public record JournalEntryLine(
    int AccountId,
    string? Description,
    decimal? Debit,
    decimal? Credit,
    int? CostCenterId,
    string? ReferenceNumber);

public record CreateJournalEntryCommand(
    int JournalType,
    int CostCenter,
    string Description,
    List<JournalEntryLine> Lines,
    string? ReferenceNumber1 = null
)
{
    public decimal Balance =>
        (Lines?.Where(e => e.Debit.HasValue).Sum(e => e.Debit) ?? 0)
        - (Lines?.Where(e => e.Credit.HasValue).Sum(e => e.Credit) ?? 0);
}

public class CreateJournalEntryCommandValidator : AbstractValidator<CreateJournalEntryCommand>
{
    public CreateJournalEntryCommandValidator()
    {
        RuleFor(e => e.JournalType).GreaterThanOrEqualTo(0);
        RuleFor(e => e.Description).MinimumLength(3).When(e => e.Description != null);
        RuleFor(e => e.Lines).NotEmpty();
        RuleForEach(e => e.Lines).SetValidator(new JournalEntryLineValidator());

        // Must total zero
        RuleFor(e => e.Balance).Equals(0);
    }
}

public class JournalEntryLineValidator : AbstractValidator<JournalEntryLine>
{
    public JournalEntryLineValidator()
    {
        RuleFor(e => e.AccountId).NotEmpty();
        RuleFor(e => e.Description).MinimumLength(3);
        //RuleFor(e => e.ReferenceNumber).NotEmpty();
        RuleFor(e => e.CostCenterId).GreaterThanOrEqualTo(0).When(e => e.CostCenterId != null);
        // Debit/Credit is mutually exclusive
        RuleFor(e => e.Debit).NotEmpty().When(e => e.Credit == null);
        RuleFor(e => e.Credit).NotEmpty().When(e => e.Debit == null); 
    }
}