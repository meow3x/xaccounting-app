using Api.Database;
using Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Features.Reports;

[Route("api/[controller]")]
[ApiController]
public class ReportsController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public ReportsController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // GET: api/<ReportsController>
    [HttpGet("Accounts/Receivable")]
    public async Task<IEnumerable<VW_AccountsReceivable>> Get()
    {
        return await _dbContext.AccountsReceivable.ToListAsync();
    }

    [HttpGet("Accounts/Payable")]
    public async Task<IEnumerable<VW_AccountsPayable>> GetAp()
    {
        return await _dbContext.AccountsPayableView.ToListAsync();
    }


    // GET api/<ReportsController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<ReportsController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/<ReportsController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<ReportsController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
