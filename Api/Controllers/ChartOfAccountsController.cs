using Api.Entities;
using Api.Features.ChartOfAccounts.Command;
using Api.Features.ChartOfAccounts.Query;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChartOfAccountsController : ControllerBase
{
    public readonly IMediator _mediator;
    public readonly ISender _sender;

    public ChartOfAccountsController(IMediator mediator, ISender sender)
    {
        _mediator = mediator;
        _sender = sender;
    }

    [HttpGet]
    public async Task<AccountView[]> Get()
    {
        return await _sender.Send(new GetAllAccountsQuery());
    }

    [HttpGet("AccountTypes")]
    public async Task<GetAccountTypes.GetAccountTypeResponse[]> GetAccountTypes()
    {
        return await _sender.Send(new GetAccountTypes.GetAllAccountTypesQuery());
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AccountView>> Get(int id)
    {
        var result = await _sender.Send(new GetAccountByIdQuery(id));
        return result.ToActionResult(this);
    }

    [HttpPost]
    public async Task<ActionResult<Account>> Post([FromBody] CreateAccountCommand command)
    {
        var result = await _mediator.Send(command);
        return result.ToActionResult(this);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Account>> Put([FromRoute] int id, [FromBody] UpdateAccountCommand command)
    {
        return (await _mediator.Send(command with { Id = id })).ToActionResult(this);
    }

    // DELETE api/<ChartOfAccountsController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
        throw new NotImplementedException();
    }
}
