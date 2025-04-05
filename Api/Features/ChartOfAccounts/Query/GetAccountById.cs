using Api.Database;
using Api.Entities;
using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.ChartOfAccounts.Query;

public record GetAccountByIdQuery(int Id) : IRequest<Result<AccountView>> { }

internal sealed class GetAccountByIdQueryHandler : IRequestHandler<GetAccountByIdQuery, Result<AccountView>>
{
    private readonly ApplicationDbContext _dbContext;

    public GetAccountByIdQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<AccountView>> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
    {
        var account = await _dbContext.Accounts
            .AsNoTracking()
            .Include(e => e.AccountType)
            .SingleOrDefaultAsync(e => e.Id == request.Id, cancellationToken);

        if (account is null) { return Result.NotFound(); }

        return new AccountView(
            account.Id,
            account.AccountId,
            account.Name,
            account.AccountType,
            account.Credit,
            account.Debit,
            account.YearEndBudget);
    }
}
