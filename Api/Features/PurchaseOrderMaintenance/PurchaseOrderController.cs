using Api.Database;
using Api.Entities;
using Api.Features.PurchaseOrderMaintenance.Command;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Features.PurchaseOrderMaintenance;

[Route("api/[controller]")]
[ApiController]
public class PurchaseOrderController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ApplicationDbContext _dbContext;

    public PurchaseOrderController(IMediator mediator, ApplicationDbContext dbContext)
    {
        _mediator = mediator;
        _dbContext = dbContext;
    }

    // GET: api/<PurchaseOrderController>
    [HttpGet]
    public IEnumerable<string> Get()
    {
        return new string[] { "value1", "value2" };
    }

    // GET api/<PurchaseOrderController>/5
    [HttpGet("{orderNumber}")]
    public async Task<ActionResult<PurchaseOrder>> Get([FromRoute] int orderNumber)
    {
        var po = await _dbContext.PurchaseOrders
            .Include(po => po.Project)
            .Include(po => po.Supplier)
            .Include(po => po.LineItems)
            .SingleOrDefaultAsync(e => e.Number == orderNumber);

        if (po == null)
        {
            return NotFound();
        }

        return Ok(po);
    }

    // POST api/<PurchaseOrderController>
    [HttpPost]
    public async Task<ActionResult<PurchaseOrder>> Post([FromBody] CreatePurchaseOrderCommand command)
    {
        return (await _mediator.Send(command)).ToActionResult(this);
    }

    /**
     * Mark purchase order as received. Creates purchase journal entry
     */
    [HttpPost("Receive/{id}")]
    public async Task<ActionResult<PurchaseOrder>> Post([FromRoute] int id, [FromBody] ReceivePurchaseOrderCommand command)
    {
        return (await _mediator.Send(command with { Id = id })).ToActionResult(this);
    }

    // PUT api/<PurchaseOrderController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<PurchaseOrderController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
