using Api.Database;
using Api.Entities;
using Api.Features.EmployeeMaintenance.Command;
using Api.Features.EmployeeMaintenance.Query;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeesController : ControllerBase
{
    public readonly IMediator _mediator;
    public readonly ApplicationDbContext _dbContext;

    public EmployeesController(IMediator mediator, ApplicationDbContext dbContext)
    {
        _mediator = mediator;
        _dbContext = dbContext;
    }

    // GET: api/<EmployeesController>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Employee>>> Get()
    {
        return (await _mediator.Send(new GetAllEmployeesQuery())).ToActionResult(this);
    }

    // GET api/<EmployeesController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Employee>> Get(int id)
    {
        var employee = await _dbContext.Employees
            .AsNoTracking()
            .SingleOrDefaultAsync(e => e.Id == id);

        if (employee is null)
        {
            return NotFound();
        }

        return Ok(employee);
    }

    [HttpGet("SalaryUnits")]
    public IEnumerable<string> GetSalaryUnits()
    {
        return Enum.GetValues<SalaryUnit>().Select(e => e.ToString()).ToList();
    }

    // POST api/<EmployeesController>
    [HttpPost]
    public async Task<ActionResult<Employee>> Post([FromBody] CreateEmployeeCommand command)
    {
        return (await _mediator.Send(command)).ToActionResult(this);
    }

    // PUT api/<EmployeesController>/5
    [HttpPut("{id}")]
    public async Task<ActionResult<Employee>> Put(int id, [FromBody] UpdateEmployeeCommand command)
    {
        return (await _mediator.Send(command with { Id = id })).ToActionResult(this);
    }

    // DELETE api/<EmployeesController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
