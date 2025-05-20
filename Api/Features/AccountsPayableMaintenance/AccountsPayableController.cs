using Api.Database;
using Api.Entities;
using Api.Features.AccountsPayableMaintenance.Command;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Features.AccountsPayableMaintenance;

[Route("api/[controller]")]
[ApiController]
public class AccountsPayableController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ApplicationDbContext _dbContext;

    public AccountsPayableController(IMediator mediator, ApplicationDbContext dbContext)
    {
        _mediator = mediator;
        _dbContext = dbContext;
    }

    // GET: api/<AccountsPayableController>
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    // GET api/<AccountsPayableController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<AccountsPayableController>
    [HttpPost]
    public async Task<ActionResult<AccountsPayable>> Post([FromBody] CreateAccountsPayableCommand command)
    {
        return (await _mediator.Send(command)).ToActionResult(this);
    }

    // PUT api/<AccountsPayableController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<AccountsPayableController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
