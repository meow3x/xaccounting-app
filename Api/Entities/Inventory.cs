using Microsoft.EntityFrameworkCore;
using NodaTime;
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
    public PurchaseOrderLineItem? LineItem { get; set; }

    // TODO: linked line item from sales

    [Column(TypeName = "timestamp with time zone")]
    public Instant Timestamp { get; set; } = SystemClock.Instance.GetCurrentInstant();

    public int StockBefore { get; set; }

    // Negative or positive
    public int Quantity { get; set; }
    public int StockAfter { get; set; }
}

// View
public class VW_ItemEndingCost
{
    public required string ItemCode { get; set; }
    public required string ItemName { get; set; }
    public decimal? Purchase { get; set; }
    public decimal? Sold { get; set; }
    public decimal? EndCost { get; set; }
}
