using Api.Entities;

namespace Api.Features.EmployeeMaintenance.Query;

using Api.Database;
using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using EmployeeList = IEnumerable<Employee>;

//public record PaginationContext(int Page, int Count, string SortBy, int SortDirection) { }

public record GetAllEmployeesQuery() : IRequest<Result<EmployeeList>> { }

internal sealed class GetAllEmployeesQueryHandler : IRequestHandler<GetAllEmployeesQuery, Result<EmployeeList>>
{
    private readonly ApplicationDbContext _dbContext;

    public GetAllEmployeesQueryHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<EmployeeList>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
    {
        // TODO: Pagination parameters

        var data = await _dbContext.Employees
            .AsNoTracking()
            .OrderBy(e => e.Id)
            .ToListAsync(cancellationToken);

        return Result<EmployeeList>.Success(data);
    }
}

