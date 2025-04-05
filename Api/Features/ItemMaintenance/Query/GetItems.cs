using Api.Database;
using Api.Entities;
using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;

namespace Api.Features.ItemMaintenance.Query;

using ItemList = IEnumerable<Item>;

//public record PaginationContext(int Page, int Count, string SortBy, int SortDirection) { }

public record GetAllItemsQuery() : IRequest<Result<ItemList>> {}

internal sealed class GetAllItemsQueryHandler : IRequestHandler<GetAllItemsQuery, Result<ItemList>>
{
    private readonly ApplicationDbContext _dbContext;

    public GetAllItemsQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<ItemList>> Handle(GetAllItemsQuery request, CancellationToken cancellationToken)
    {
        // TODO: Pagination parameters

        var data = await _dbContext.Items
            .AsNoTracking()
            .Include(e => e.Uom)
            .Include(e => e.Category)
            .OrderBy(e => e.Name)
            .ToListAsync(cancellationToken);

        return Result<ItemList>.Success(data);
    }
}

