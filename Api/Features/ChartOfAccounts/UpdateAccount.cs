using Api.Database;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.ChartOfAccounts;

public static class UpdateAccount
{
    public record UpdateAccountCommand(
        int Id,
        string AccountId,
        string Name,
        int AccountTypeId) : IRequest<int> { }

    internal sealed class Handler : IRequestHandler<UpdateAccountCommand, int>
    {
        private readonly ApplicationDbContext _dbContext;

        public Handler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
        {
            var account = await _dbContext.Accounts
                .SingleOrDefaultAsync(e => e.Id == request.Id, cancellationToken)
                    ?? throw new ArgumentOutOfRangeException(nameof(request), "Not found");
            var accountType = await _dbContext.AccountTypes
                .AsNoTracking()
                .SingleOrDefaultAsync(e => e.Id == request.AccountTypeId, cancellationToken)
                    ?? throw new ArgumentOutOfRangeException(nameof(request), "Account Type not found");

            account.AccountId = request.AccountId;
            account.Name = request.Name;
            account.AccountType = accountType;

            await _dbContext.SaveChangesAsync(cancellationToken);

            return 0;
        }
    }
}
