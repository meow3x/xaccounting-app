using Api.Database;
using Api.Entities;
using Api.Features.Shared.Dto;
using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Api.Features.EmployeeMaintenance.Command;

public record CreateEmployeeCommand(
    string EmployeeId,
    string FirstName,
    string LastName,
    string? MiddleName,
    AddressDto Address,
    string? Tin,
    string? PagIbigId,
    string? PhilhealthId,
    decimal? Rate,
    SalaryUnit SalaryUnit) : IRequest<Result<Employee>>;

public class CreateEmployeeCommandValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator()
    {
        RuleFor(e => e.EmployeeId).Length(3, 255);
        RuleFor(e => e.FirstName).NotEmpty();
        RuleFor(e => e.LastName).NotEmpty();
        //RuleFor(e => e.MiddleName);
        RuleFor(e => e.Address).SetValidator(new AddressDtoValidator());
        RuleFor(e => e.Tin).MinimumLength(3).When(e => e.Tin != null); // TODO: Regex
        RuleFor(e => e.PagIbigId).MinimumLength(3).When(e =>  e.PagIbigId != null);
        RuleFor(e => e.PhilhealthId).MinimumLength(3).When(e => e.PhilhealthId != null);
        RuleFor(e => e.Rate).GreaterThanOrEqualTo(0).When(e => e.Rate != null);
        RuleFor(e => e.SalaryUnit).IsInEnum();
    }
}

internal class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, Result<Employee>>
{
    private readonly ApplicationDbContext _dbContext;

    public CreateEmployeeCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Employee>> Handle(CreateEmployeeCommand command, CancellationToken cancellationToken)
    {
        var validation = (new CreateEmployeeCommandValidator()).Validate(command);

        // employe id not taken
        bool employeeIdTaken = await _dbContext.Employees.AnyAsync(e => e.EmployeeId == command.EmployeeId, cancellationToken);
        if (employeeIdTaken) { validation.Errors.Add(new ValidationFailure(nameof(command.EmployeeId), "Employee id already exists"));  }

        if (!validation.IsValid) { return Result<Employee>.Invalid(validation.AsErrors()); }

        var employee = new Employee
        {
            EmployeeId = command.EmployeeId,
            FirstName = command.FirstName,
            LastName = command.LastName,
            MiddleName = command.MiddleName,
            Address = new Address(
                Street: command.Address.Street,
                City: command.Address.City,
                Province: command.Address.Province,
                LandlineNumber: command.Address.LandlineNumber,
                MobileNumber: command.Address.MobileNumber),
            Tin = command.Tin,
            PagIbigId = command.PagIbigId,
            PhilhealthId = command.PhilhealthId,
            Rate = command.Rate,
            SalaryUnit = command.SalaryUnit,
        };

        await _dbContext.Employees.AddAsync(employee, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result<Employee>.Success(employee);
    }
}