using Api.Database;
using Api.Entities;
using Api.Features.SupplierMaintenance.Command;
using Api.Features.SupplierMaintenance.Query;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SuppliersController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ApplicationDbContext _dbContext;

    public SuppliersController(IMediator mediator, ApplicationDbContext dbContext)
    {
        _mediator = mediator;
        _dbContext = dbContext;
    }

    // GET: api/<SuppliersController>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Supplier>>> Get()
    {
        return (await _mediator.Send(new GetAllSuppliersQuery())).ToActionResult(this);
    }

    // GET api/<SuppliersController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Supplier>> Get(int id)
    {
        var supplier = await _dbContext.Suppliers
            .Include(e => e.PaymentTerm)
            .AsNoTracking()
            .SingleOrDefaultAsync(e => e.Id == id);

        if (supplier is null)
        {
            return NotFound();
        }

        return Ok(supplier);
    }

    // POST api/<SuppliersController>
    [HttpPost]
    public async Task<ActionResult<Supplier>> Post([FromBody] CreateSupplierCommand request)
    {
        return (await _mediator.Send(request)).ToActionResult(this);
    }

    // PUT api/<SuppliersController>/5
    [HttpPut("{id}")]
    public async Task<ActionResult<Supplier>> Put(int id, [FromBody] UpdateSupplierCommand command)
    {
        return (await _mediator.Send(command with { Id = id })).ToActionResult(this);
    }

    // DELETE api/<SuppliersController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
