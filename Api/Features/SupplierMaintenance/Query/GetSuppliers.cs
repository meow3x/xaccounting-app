using Api.Database;
using Api.Entities;
using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.SupplierMaintenance.Query;

using SuppliersList = IEnumerable<Supplier>;

public record GetAllSuppliersQuery() : IRequest<Result<SuppliersList>>;

internal sealed class GetAllSuppliersQueryHandler : IRequestHandler<GetAllSuppliersQuery, Result<SuppliersList>>
{
    public readonly ApplicationDbContext _dbContext;

    public GetAllSuppliersQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<SuppliersList>> Handle(GetAllSuppliersQuery request, CancellationToken cancellationToken)
    {
        return Result<SuppliersList>.Success(
            await _dbContext.Suppliers
                .Include(e => e.PaymentTerm)
                .AsNoTracking()
                .ToListAsync(cancellationToken));
    }
}
