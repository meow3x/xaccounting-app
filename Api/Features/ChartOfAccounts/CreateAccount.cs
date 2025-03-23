using Api.Database;
using Api.Entities;
using MediatR;
using System.Diagnostics;
using System.Xml.Linq;

namespace Api.Features.ChartOfAccounts;

public static class CreateAccount
{
    public record CreateAccountCommand(
        string AccountId,
        string Name,
        int AccountTypeId) : IRequest<int>;

    internal sealed class Handler : IRequestHandler<CreateAccountCommand, int>
    {
        private readonly ApplicationDbContext _dbContext;

        public Handler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<int> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            // Check account type
            var accountType = _dbContext.AccountTypes.FirstOrDefault(e => e.Id == request.AccountTypeId)
                ?? throw new ArgumentOutOfRangeException(nameof(request));

            var account = new Account
            {
                AccountId = request.AccountId,
                Name = request.Name,
                AccountType = accountType
            };

            _dbContext.Add(account);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return account.Id;
        }
    }
}
