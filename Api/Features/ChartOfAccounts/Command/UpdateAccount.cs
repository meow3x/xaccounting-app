using Api.Database;
using Api.Entities;
using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Api.Features.ChartOfAccounts.Command;

public record UpdateAccountCommand(
    [property: JsonIgnore] int Id,
    string AccountId, // FIXME: Should this be updateable ?
    string Name,
    int AccountTypeId) : IRequest<Result<Account>> { }

public class UpdateAccountCommandValidator : AbstractValidator<UpdateAccountCommand>
{
    public UpdateAccountCommandValidator()
    {
        RuleFor(x => x.AccountId).MinimumLength(3);
        RuleFor(x => x.Name).MinimumLength(3);
        RuleFor(x => x.AccountTypeId).GreaterThan(0);
    }
}

internal sealed class UpdateAccountHandler : IRequestHandler<UpdateAccountCommand, Result<Account>>
{
    private readonly ApplicationDbContext _dbContext;

    public UpdateAccountHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Account>> Handle(UpdateAccountCommand command, CancellationToken cancellationToken)
    {
        var validator = new UpdateAccountCommandValidator();
        var validation = validator.Validate(command);
        
        var account = await _dbContext.Accounts
            .Include(a => a.AccountType)
            .SingleOrDefaultAsync(e => e.Id == command.Id, cancellationToken);
        var accountType = await _dbContext.AccountTypes
           .SingleOrDefaultAsync(e => e.Id == command.AccountTypeId, cancellationToken);
        
        if (account is null) { validation.Errors.Add(new ValidationFailure(nameof(command.Id), "Account not found")); }
        if (accountType is null) { validation.Errors.Add(new ValidationFailure(nameof(command.AccountTypeId), "Account type not found")); }
        if (!validation.IsValid) { return Result<Account>.Invalid(validation.AsErrors()); }

        account!.AccountId = command.AccountId;
        account.Name = command.Name;
        account.AccountType = accountType!;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result<Account>.Success(account);
    }
}
