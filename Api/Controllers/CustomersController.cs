using Api.Entities;
using Api.Features.CustomerMaintenance.Command;
using Api.Features.CustomerMaintenance.Query;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly IMediator _mediator;

    public CustomersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: api/<CustomersController>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Customer>>> Get()
    {
        return (await _mediator.Send(new GetAllCustomersQuery())).ToActionResult(this);

    }

    // GET api/<CustomersController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<CustomersController>
    [HttpPost]
    public async Task<ActionResult<Customer>> Post([FromBody] CreateCustomerCommand command)
    {
        return (await _mediator.Send(command)).ToActionResult(this);
    }

    // PUT api/<CustomersController>/5
    [HttpPut("{id}")]
    public async Task<ActionResult<Customer>> Put(int id, [FromBody] UpdateCustomerCommand command)
    {
        return (await _mediator.Send(command with { Id = id })).ToActionResult(this);
    }

    // DELETE api/<CustomersController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
