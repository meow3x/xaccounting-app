using Api.Database;
using Api.Entities;
using Api.Features.Shared.Dto;
using Ardalis.Result;
using Ardalis.Result.FluentValidation;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Api.Features.EmployeeMaintenance.Command;

public record UpdateEmployeeCommand(
    [property: JsonIgnore] int Id,
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

public class UpdateEmployeeCommandValidator : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandValidator()
    {
        RuleFor(e => e.EmployeeId).Length(3, 255);
        RuleFor(e => e.FirstName).NotEmpty();
        RuleFor(e => e.LastName).NotEmpty();
        //RuleFor(e => e.MiddleName);
        RuleFor(e => e.Address).SetValidator(new AddressDtoValidator());
        RuleFor(e => e.Tin).MinimumLength(3).When(e => e.Tin != null); // TODO: Regex
        RuleFor(e => e.PagIbigId).MinimumLength(3).When(e => e.PagIbigId != null);
        RuleFor(e => e.PhilhealthId).MinimumLength(3).When(e => e.PhilhealthId != null);
        RuleFor(e => e.Rate).GreaterThanOrEqualTo(0).When(e => e.Rate != null);
        RuleFor(e => e.SalaryUnit).IsInEnum();
    }
}

internal class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, Result<Employee>>
{
    private readonly ApplicationDbContext _dbContext;

    public UpdateEmployeeCommandHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<Employee>> Handle(UpdateEmployeeCommand command, CancellationToken cancellationToken)
    {
        var validation = (new UpdateEmployeeCommandValidator()).Validate(command);

        var employee = await _dbContext.Employees.FindAsync(command.Id);
        if (employee is null) { validation.Errors.Add(new ValidationFailure(nameof(command.Id), "Employee not found"));  }

        bool employeeIdTaken = await _dbContext.Employees.AnyAsync(
            e => e.Id != command.Id && e.EmployeeId == command.EmployeeId,
            cancellationToken);
        if (employeeIdTaken) { validation.Errors.Add(new ValidationFailure(nameof(command.EmployeeId), "Employee id already exists")); }

        if (!validation.IsValid) { return Result<Employee>.Invalid(validation.AsErrors()); }

        employee.EmployeeId = command.EmployeeId;
        employee.FirstName = command.FirstName;
        employee.LastName = command.LastName;
        employee.MiddleName = command.MiddleName;
        employee.Address = new Address(
             command.Address.Street,
             command.Address.City,
             command.Address.Province,
             command.Address.LandlineNumber,
             command.Address.MobileNumber);
        employee.Tin = command.Tin;
        employee.PagIbigId = command.PagIbigId;
        employee.PhilhealthId = command.PhilhealthId;
        employee.Rate = command.Rate;
        employee.SalaryUnit = command.SalaryUnit;
    
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result<Employee>.Success(employee);
    }
}