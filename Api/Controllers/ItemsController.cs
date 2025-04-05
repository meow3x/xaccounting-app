using Api.Entities;
using Api.Features.ItemMaintenance.Command;
using Api.Features.ItemMaintenance.Query;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ItemsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ItemsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET: api/<ItemController>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Item>>> Get()
    {
        return (await _mediator.Send(new GetAllItemsQuery())).ToActionResult(this);
    }

    // GET api/<ItemController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Item>> Get(int id)
    {
        return (await _mediator.Send(new GetItemByIdQuery(id))).ToActionResult(this);
    }

    // POST api/<ItemController>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Item>> Post([FromBody] CreateItemCommand command)
    {
        return (await _mediator.Send(command)).ToActionResult(this);
    }

    // PATCH api/<ItemController>/5
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Item>> Put([FromRoute] int id, [FromBody] UpdateItemCommand command)
    {
        return (await _mediator.Send(command with { Id = id })).ToActionResult(this);
    }

    // DELETE api/<ItemController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
