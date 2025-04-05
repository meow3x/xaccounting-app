using Api.Entities;
using Api.Database;
using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.CustomerMaintenance.Query;

using CustomersList = IEnumerable<Customer>;

public record GetAllCustomersQuery() : IRequest<Result<CustomersList>>;

internal sealed class GetAllCustomersQueryHandler : IRequestHandler<GetAllCustomersQuery, Result<CustomersList>>
{
    public readonly ApplicationDbContext _dbContext;

    public GetAllCustomersQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<CustomersList>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
    {
        return Result<CustomersList>.Success(
            await _dbContext.Customers
                .Include(e => e.PaymentTerm)
                .AsNoTracking()
                .ToListAsync(cancellationToken));
    }
}
