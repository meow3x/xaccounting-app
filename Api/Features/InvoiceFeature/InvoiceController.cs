using Api.Database;
using Api.Entities;
using Api.Features.Collections.Command;
using Api.Features.InvoiceFeature.Command;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Features.InvoiceFeature;

[Route("api/[controller]")]
[ApiController]
public class InvoiceController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ApplicationDbContext _dbContext;

    public InvoiceController(IMediator mediator, ApplicationDbContext dbContext)
    {
        _mediator = mediator;
        _dbContext= dbContext;
    }

    // GET: api/<InvoiceController>
    [HttpGet]
    public async Task<IEnumerable<Invoice>> Get()
    {
        return await _dbContext.Invoices.AsNoTracking()
            .Include(e => e.Customer)
            .Include(e => e.JournalEntry)
               .ThenInclude(je => je.Lines)
               .ThenInclude(jl => jl.CostCenter)
           .Include(e => e.JournalEntry)
               .ThenInclude(je => je.Lines)
               .ThenInclude(jl => jl.Account)
           .Include(e => e.JournalEntry)
               .ThenInclude(je => je.Lines)
               .ThenInclude(jl => jl.Account)
               .ThenInclude(a => a.AccountType)
           .Include(e => e.JournalEntry)
               .ThenInclude(je => je.JournalType)
            .ToListAsync();
    }

    [HttpGet("OnAccount")]
    public async Task<IEnumerable<Invoice>> GetOnAccounts()
    {
        return await _dbContext.Invoices.AsNoTracking()
            .Include(e => e.Customer)
            .Include(e => e.LineItems)
            .Include(e => e.Payments)
                .ThenInclude(e => e.JournalEntry)
                    .ThenInclude(e => e.JournalType)
            .Include(e => e.Payments)
                .ThenInclude(e => e.JournalEntry)
                    .ThenInclude(e => e.Lines)
                        .ThenInclude(e => e.Account)
                            .ThenInclude(e => e.AccountType)
            .Include(e => e.JournalEntry)
                .ThenInclude(je => je.Lines)
                    .ThenInclude(jl => jl.CostCenter)
            .Include(e => e.JournalEntry)
                .ThenInclude(je => je.Lines)
                    .ThenInclude(jl => jl.Account)
            .Include(e => e.JournalEntry)
                .ThenInclude(je => je.Lines)
                    .ThenInclude(jl => jl.Account)
                        .ThenInclude(a => a.AccountType)
            .Include(e => e.JournalEntry)
                .ThenInclude(je => je.JournalType)
            .Where(e => e.PaymentMethod == PaymentMethod.OnAccount)
            .ToListAsync();
    }

    [HttpGet("OnAccount/{invoiceNumber}")]
    public async Task<ActionResult<Invoice>> GetOnAccountByNumber(int invoiceNumber)
    {
        var invoice = await _dbContext.Invoices.AsNoTracking()
            .Include(e => e.Customer)
            .Include(e => e.JournalEntry)
               .ThenInclude(je => je.Lines)
               .ThenInclude(jl => jl.CostCenter)
           .Include(e => e.JournalEntry)
               .ThenInclude(je => je.Lines)
               .ThenInclude(jl => jl.Account)
           .Include(e => e.JournalEntry)
               .ThenInclude(je => je.Lines)
               .ThenInclude(jl => jl.Account)
               .ThenInclude(a => a.AccountType)
           .Include(e => e.JournalEntry)
               .ThenInclude(je => je.JournalType)
            .SingleOrDefaultAsync(e => e.Number == invoiceNumber);

        return invoice == null ? NotFound() : Ok(invoice);
    }

    // GET api/<InvoiceController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    [HttpPost("Payment")]
    public async Task<ActionResult<CollectionPayment>> PostColPayment([FromBody] PostColPaymentCommand command)
    {
        return (await _mediator.Send(command)).ToActionResult(this);
    }

    // POST api/<InvoiceController>
    [HttpPost]
    public async Task<ActionResult<Invoice>> Post([FromBody] CreateInvoiceCommand command)
    {
        return (await _mediator.Send(command)).ToActionResult(this);
    }

    // PUT api/<InvoiceController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<InvoiceController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
