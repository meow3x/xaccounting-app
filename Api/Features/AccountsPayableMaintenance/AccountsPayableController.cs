using Api.Database;
using Api.Entities;
using Api.Features.AccountsPayableMaintenance.Command;
using Ardalis.Result.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
   
    // GET api/<AccountsPayableController>/5

    [HttpGet("CostCenters")]
    public async Task<IEnumerable<CostCenter>> GetCostCenters(CancellationToken cancellationToken)
    {
        return await _dbContext.CostCenters.AsNoTracking().ToListAsync(cancellationToken);
    }

    [HttpGet("{voucherNumber}")]
    public async Task<ActionResult<AccountsPayable>> GetByVoucher(int voucherNumber)
    {
        var ap = await _dbContext.AccountsPayable
           .AsNoTracking()
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
           .Include(e => e.Supplier)
           .Where(e => e.VoucherNumber == voucherNumber)
           .FirstOrDefaultAsync();

        return ap != null ? Ok(ap) : NotFound();
    }

    [HttpGet("Vouchers")]
    public async Task<IEnumerable<int>> GetVouchers()
    {
        return await _dbContext.AccountsPayable.AsNoTracking().Select(e => e.VoucherNumber).ToListAsync();
    }

    [HttpGet]
    public async Task<IEnumerable<AccountsPayable>> Get()
    {
        return await _dbContext.AccountsPayable
            .AsNoTracking()
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
            .Include(e => e.Supplier)
            .OrderByDescending(e => e.Id)
            .ToListAsync();
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
