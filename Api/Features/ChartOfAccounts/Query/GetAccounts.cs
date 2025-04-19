using Api.Database;
using Api.Entities;
using Api.Features.Shared;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QueryParser;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace Api.Features.ChartOfAccounts.Query;

using PagedAccounts = PagedResult<Account>;

public record PagedResult<T>(int Total, int? Page, int? PageSize, IEnumerable<T> Data);

public record AccountView(
    int Id,
    string AccountId,
    string Name,
    AccountType AccountType,
    decimal Debit,
    decimal Credit,
    decimal YearEndBudget);

public record AccountFilters(
    string? AccountId,
    string? Name,
    int[]? AccountTypes);

public record GetAllAccountsQuery(
    IEnumerable<SearchTerm>? Filters,
    SortCriteria? Sort,
    PageOptions? PageOptions) : IRequest<Result<PagedAccounts>> {}


internal sealed class Handler : IRequestHandler<GetAllAccountsQuery, Result<PagedAccounts>>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger _logger;

    public Handler(ApplicationDbContext dbContext, ILogger<Handler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task<Result<PagedAccounts>> Handle(GetAllAccountsQuery query, CancellationToken cancellationToken)
    {
        //_logger.LogInformation(JsonSerializer.Serialize(query.PageOptions, new JsonSerializerOptions { WriteIndented = true }));
        _logger.LogInformation("Page options {0}", query.PageOptions?.ToString());
        _logger.LogInformation(string.Join(", ", query.Filters ?? []));

        var searchModel = new EntitySearchModelBuilder<Account>()
            .MapField("account_type", e => e.AccountType.Id)
            .MapField("account_id", e => e.AccountId)
            .MapField("name", e => e.Name);

        //var engine = searchModelBuilder.Build();

        // Fixme: Pagination
        IQueryable<Account> searchQuery = _dbContext.Accounts
            .AsNoTracking()
            .Include(e => e.AccountType)
            .AdvancedSearch(query.Filters, searchModel)
            .OrderBy(e => e.Id);
            //.Where(e => query.PageOptions == null || e.Id > query.PageOptions.PageId)

        var resultCount = await searchQuery.CountAsync(cancellationToken);
        var result = await searchQuery
            .Skip(query.PageOptions)
            .Take(query.PageOptions?.Limit ?? 30)
            .ToListAsync(cancellationToken);

        return Result.Success(new PagedAccounts(
            resultCount,
            query.PageOptions?.PageId,
            query.PageOptions?.Limit,
            result));
    }
}   

