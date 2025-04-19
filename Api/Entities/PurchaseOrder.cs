using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Api.Entities;

[Owned]
public class ItemSnapshot
{
    [MaxLength(25)]
    public required string Code { get; set; }

    [MaxLength(512)]
    public required string Name { get; set; }
    public required int UnitOfMeasurementId { get; set; }
    public required string UnitOfMeasurement { get; set; }
    //public decimal UnitPrice { get; set; }
    public decimal UnitCost { get; set; }
    //public decimal Discount { get; set; }
    //public decimal Total { get; set; }
}

// Immutable
public class LineItem : BaseEntity
{
    public static LineItem FromItem(int quantity, decimal discount, Item item)
    {
        return new LineItem
        {
            OriginalItem = item,
            ItemSnapshot = new ItemSnapshot
            {
                Code = item.Code,
                Name = item.Name,
                UnitOfMeasurementId = item.Uom.Id,
                UnitOfMeasurement = item.Uom.Name,
                //UnitPrice = item.UnitPrice,
                UnitCost = item.UnitCost,
                //Discount = requestLineItem.Discount
            },
            Quantity = quantity,
            Discount = discount,
            LineTotal = (quantity * item.UnitCost) /*- discount */
        };
    }

    public int PurchaseOrderId { get; set; }
    public required Item OriginalItem { get; set; }
    public required ItemSnapshot ItemSnapshot { get; set; }
    public int Quantity { get; set; }
    public decimal Discount { get; set; }
    public decimal LineTotal { get; set; }

}

[Index(nameof(Number), IsUnique = true)]
public class Project : BaseEntity
{
    public required string Number { get; set; }
    public required string Name { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter<OrderStatus>))]
public enum OrderStatus : byte
{
    Closed = 0,
    Open = 1
}

[Index(nameof(Number), IsUnique = true)]
public class PurchaseOrder : BaseEntity
{
    public int Number { get; set; } // Auto generatedd

    [MaxLength(255)]
    public string? RRNumber { get; set; }
    [MaxLength(255)]
    public required string RequisitionNumber { get; set; }
    public required DateOnly CreatedAtDate { get; set; } // Should we just use Created at instead
    public required DateOnly DeliveryDate { get; set; }
    public required Project Project { get; set; }
    public required string Description { get; set; }
    public required Supplier Supplier { get; set; }
    public ICollection<LineItem> LineItems { get; set; } = [];
    public decimal VatableAmount { get; set; }
    public decimal VatAmount { get; set; }
    public decimal Discounted { get; set; }
    public decimal NetAmount { get; set; }

    public OrderStatus Status { get; set; } = OrderStatus.Open;

    [Column(TypeName = "timestamp without time zone")]
    public DateTime? ClosedAt { get; set; } = null;

    // Journal Posting
    public int DebitTo { get; set; }
    public int CreditTo { get; set; } // Is this even used
    //public required JournalType JournalType { get; set; }
}
