using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Entities;

//[Index(nameof(Item))]
public class Inventory : BaseEntity
{
    public required Item Item { get; set; }
    public int Stock { get; set; }

    public ICollection<InventoryLog> Logs { get; set; } = [];

    [Timestamp]
    public uint Version { get; set; } // Concurrency token
}

// Append only ledger
public class InventoryLog
{
    public int Id { get; set; }
    public required Inventory Inventory { get; set; }

    // Linked line item from purchase order
    public LineItem? LineItem { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column(TypeName = "date")]
    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public int StockBefore { get; set; }

    // Negative or positive
    public int Quantity { get; set; }
    public int StockAfter { get; set; }
}