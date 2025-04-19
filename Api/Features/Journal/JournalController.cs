using Api.Database;
using Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Features.Journal;

[Route("api/[controller]")]
[ApiController]
public class JournalController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public JournalController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // GET: api/<JournalController>
    [HttpGet]
    public async Task<IEnumerable<JournalEntry>> Get(
        [FromQuery(Name = "page_id")] int? pageId,
        [FromQuery(Name = "page_size")] int? pageSize
    )
    {
        return await _dbContext.JournalEntries
            .AsNoTracking()
            .Include(e => e.JournalType)
            .Include(e => e.Lines)
            .ThenInclude(e => e.Account)
            .Skip((pageId.GetValueOrDefault(1) - 1) * pageSize.GetValueOrDefault(30))
            .Take(pageSize.GetValueOrDefault(30))
            .ToListAsync();
    }

    // GET api/<JournalController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<JournalController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/<JournalController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<JournalController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
