using Api.Database;
using Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Features.InventoryMaintenance;

[Route("api/[controller]")]
[ApiController]
public class InventoryController : ControllerBase
{
    private readonly ApplicationDbContext _dbContext;

    public InventoryController(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    // GET: api/<InventoryController>
    [HttpGet("Views/Quantity")]
    public async Task<IEnumerable<object>> GetQuantity()
    {
        var query = from item in _dbContext.Items
                join i in _dbContext.Inventory
                    on item.Id equals i.Item.Id into grouping
                from i in grouping.DefaultIfEmpty()
                select new
                {
                    Id = item.Id,
                    ItemCode = item.Code,
                    Name = item.Name,
                    Quantity = i != null ? i.Stock : 0
                };

        return await query.ToListAsync();
    }

    [HttpGet("Views/Quantity/{itemId}")]
    public async Task<object> GetItemQuantityBalance(int itemId)
    {
        var query = from item in _dbContext.Items
                    join i in _dbContext.Inventory
                        on item.Id equals i.Item.Id into grouping
                    from i in grouping.DefaultIfEmpty()
                    where item.Id == itemId
                    select new
                    {
                        Id = item.Id,
                        ItemCode = item.Code,
                        Name = item.Name,
                        Quantity = i != null ? i.Stock : 0
                    };
        return await query.SingleAsync();
    }

    [HttpGet("Views/EndingCost")]
    public async Task<IEnumerable<VW_ItemEndingCost>> GetEndingInventoryCost()
    {
        return await _dbContext.EndingCostView.ToListAsync();
    }

    // GET api/<InventoryController>/5
    [HttpGet("{id}")]
    public string Get(int id)
    {
        return "value";
    }

    // POST api/<InventoryController>
    [HttpPost]
    public void Post([FromBody] string value)
    {
    }

    // PUT api/<InventoryController>/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] string value)
    {
    }

    // DELETE api/<InventoryController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
