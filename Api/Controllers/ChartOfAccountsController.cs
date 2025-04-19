using Api.Entities;
using Api.Features.ChartOfAccounts.Command;
using Api.Features.ChartOfAccounts.Query;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Api.Features.Shared.Dto;
using Api.Features.Shared;
using System.Text.Json;
using QueryParser;
using System.Reflection.Metadata.Ecma335;

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
    public async Task<ActionResult<IEnumerable<Account>>> Get(
        [FromQuery(Name = "page_id")] int? pageId,
        [FromQuery(Name = "page_size")] int? pageSize,
        [FromQuery] string? q)
    {
        var request = new GetAllAccountsQuery(
            string.IsNullOrEmpty(q) ? null : SearchQueryParser.Parse(q),
            null,
            new PageOptions(pageId ?? 1, pageSize ?? 30));

        var result = await _sender.Send(request);

        Response.Headers["X-Pagination-Total"] = result.Value.Total.ToString();
        Response.Headers["X-Pagination-Page"] = result.Value.Page.ToString();
        Response.Headers["X-Pagination-Page-Size"] = result.Value.PageSize.ToString();

        return Ok(result.Value.Data);
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
