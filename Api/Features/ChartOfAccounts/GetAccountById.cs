using Api.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.ChartOfAccounts;

public static class GetAccountById
{
    public record GetAccountByIdQuery(int Id) : IRequest<GetAccountResponse> { }

    internal sealed class Handler : IRequestHandler<GetAccountByIdQuery, GetAccountResponse>
    {
        private readonly ApplicationDbContext _dbContext;

        public Handler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetAccountResponse> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
        {
            var account = await _dbContext.Accounts
                .AsNoTracking()
                .Include(e => e.AccountType)
                .SingleOrDefaultAsync(e => e.Id == request.Id, cancellationToken)
                    ?? throw new ArgumentOutOfRangeException(nameof(request), "Resource not found");

            return new GetAccountResponse(
                account.Id,
                account.AccountId,
                account.Name,
                account.AccountType,
                account.Credit,
                account.Debit,
                account.YearEndBudget);
        }
    }
}

