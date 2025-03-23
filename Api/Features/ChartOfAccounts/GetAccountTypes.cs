using Api.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.ChartOfAccounts;

public static class GetAccountTypes
{
    public record GetAccountTypeResponse(int Id, string Label);

    public record GetAllAccountTypesQuery() : IRequest<GetAccountTypeResponse[]> {}

    sealed class Handler : IRequestHandler<GetAllAccountTypesQuery, GetAccountTypeResponse[]>
    {
        private readonly ApplicationDbContext _dbContext;

        public Handler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<GetAccountTypeResponse[]> Handle(GetAllAccountTypesQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.AccountTypes
                .AsNoTracking()
                .OrderBy(e => e.Id)
                .Select(e => new GetAccountTypeResponse(e.Id, e.Label!))
                .ToArrayAsync(cancellationToken);
        }
    }
}
