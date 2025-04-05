using Api.Database;
using Api.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.ChartOfAccounts.Query;

public record AccountView(
    int Id,
    string AccountId,
    string Name,
    AccountType AccountType,
    decimal Debit,
    decimal Credit,
    decimal EndBudget);

public record GetAllAccountsQuery : IRequest<AccountView[]> {}

internal sealed class Handler : IRequestHandler<GetAllAccountsQuery, AccountView[]>
{
    private readonly ApplicationDbContext _dbContext;

    public Handler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<AccountView[]> Handle(GetAllAccountsQuery request, CancellationToken cancellationToken)
    {
        // Fixme: Pagination
        return await _dbContext.Accounts
            .AsNoTracking()
            .OrderBy(e => e.Id)
            .Select(e => new AccountView(e.Id, e.AccountId, e.Name, e.AccountType, e.Credit, e.Debit, e.YearEndBudget))
            .ToArrayAsync(cancellationToken);
    }
}