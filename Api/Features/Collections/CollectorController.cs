using Api.Entities;
using Api.Features.Collections.Command;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Features.Collections;

[Route("api/[controller]")]
[ApiController]
public class CollectorController : ControllerBase
{
    private readonly IMediator _mediator;

    public CollectorController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: api/<CollectorController>
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    // GET api/<CollectorController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<CollectorController>
    [HttpPost("Payment")]
    public async Task<ActionResult<CollectionPayment>> Post(PostColPaymentCommand command)
    {
        return (await _mediator.Send(command)).ToActionResult(this);
    }

    // PUT api/<CollectorController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<CollectorController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
