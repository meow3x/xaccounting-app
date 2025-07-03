using Api.Database;
using Api.Entities;
using Api.Features.PaymentMaintenance.Command;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Features.PaymentMaintenance;

[Route("api/[controller]")]
[ApiController]
public class PaymentsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ApplicationDbContext _dbContext;

    public PaymentsController(IMediator mediator, ApplicationDbContext dbContext)
    {
        _mediator = mediator;
        _dbContext = dbContext;
    }

    // POST api/<PaymentsController>
    [HttpPost]
    public async Task<ActionResult<Payment>> Post([FromBody] CreatePaymentCommand command)
    {
        return (await _mediator.Send(command)).ToActionResult(this);
    }

    [HttpGet]
    public async Task<IEnumerable<Payment>> Get()
    {
        return await _dbContext.Payments.AsNoTracking()
            .Include(e => e.Payee)
            .Include(e => e.JournalEntry)
                .ThenInclude(je => je.Lines)
            .Include(e => e.JournalEntry)
                .ThenInclude(je => je.Lines)
                    .ThenInclude(jl => jl.Account)
            .Include(e => e.JournalEntry)
                .ThenInclude(je => je.Lines)
                    .ThenInclude(jl => jl.Account)
                    .ThenInclude(a => a.AccountType)
            .Include(e => e.JournalEntry)
                .ThenInclude(je => je.JournalType)
            .OrderByDescending(e => e.Id)
            .ToListAsync();
    }
}
