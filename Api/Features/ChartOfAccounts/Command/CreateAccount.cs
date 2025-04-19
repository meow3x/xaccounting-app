using Api.Database;
using Api.Entities;
using Api.Features.Shared;
using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore.Storage;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Xml.Linq;

namespace Api.Features.ChartOfAccounts.Command;

public record CreateAccountCommand(
    string AccountId,
    string Name,
    int AccountTypeId,
    decimal YearEndBudget = 0) : IRequest<Result<Account>>;

public class CreateAccountCommandValidator : AbstractValidator<CreateAccountCommand>
{
    public CreateAccountCommandValidator()
    {
        RuleFor(x => x.AccountId).NotEmpty();
        RuleFor(x => x.Name).NotEmpty();
    }
}

internal sealed class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, Result<Account>>
{
    private readonly ApplicationDbContext _dbContext;

    public CreateAccountCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Account>> Handle(CreateAccountCommand command, CancellationToken cancellationToken)
    {
        var validator = new CreateAccountCommandValidator();
        var validation = validator.Validate(command);
        
        if (!validation.IsValid)
        {
            return Result<Account>.Invalid(validation.AsErrors());
        }

        var accountType = _dbContext.AccountTypes.FirstOrDefault(e => e.Id == command.AccountTypeId);
        
        if (accountType is null)
        {
            return Result<Account>.Invalid([
                new ValidationError(nameof(command.AccountTypeId), "Account type does not exist")
            ]);
        }

        var account = new Account
        {
            AccountId = command.AccountId,
            Name = command.Name,
            AccountType = accountType
        };

        _dbContext.Add(account);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result<Account>.Created(account);
    }
}
