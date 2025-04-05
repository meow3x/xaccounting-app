using Api.Database;
using Api.Entities;
using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Api.Features.ItemMaintenance.Query;

public record GetItemByIdQuery(int Id) : IRequest<Result<Item>> { }

internal sealed class GetItemByQueryHandler : IRequestHandler<GetItemByIdQuery, Result<Item>>
{
    private readonly ApplicationDbContext _dbContext;

    public GetItemByQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Item>> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
    {
        var data = await _dbContext.Items
            .AsNoTracking()
            .Include(e => e.Uom)
            .Include(e => e.Category)
            .SingleOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
        return data != null ? Result<Item>.Success(data) : Result<Item>.NotFound();
    }
}

