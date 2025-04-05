using Api.Entities;
using Api.Features.EmployeeMaintenance.Command;
using Api.Features.EmployeeMaintenance.Query;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeesController : ControllerBase
{
    public readonly IMediator _mediator;

    public EmployeesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: api/<EmployeesController>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Employee>>> Get()
    {
        return (await _mediator.Send(new GetAllEmployeesQuery())).ToActionResult(this);
    }

    // GET api/<EmployeesController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
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
