using Microsoft.EntityFrameworkCore;
using NodaTime;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Api.Entities;

[Index(nameof(Number), IsUnique = true)]
public class PurchaseOrder : BaseEntity
{
    public int Number { get; set; } // Auto generatedd

    [MaxLength(255)]
    public string? RRNumber { get; set; }
    [MaxLength(255)]
    public required string RequisitionNumber { get; set; }
    public required LocalDate DeliveryDate { get; set; }
    public required Project Project { get; set; }
    public required string Description { get; set; }
    public required Supplier Supplier { get; set; }
    public ICollection<PurchaseOrderLineItem> LineItems { get; set; } = [];
    public decimal VatableAmount { get; set; }
    public decimal VatAmount { get; set; }
    public decimal Discounted { get; set; }
    public decimal NetAmount { get; set; } // FIXME: Gross - Discounted

    public OrderStatus Status { get; set; } = OrderStatus.Open;

    [Column(TypeName = "timestamp with time zone")]
    public Instant? ClosedAt { get; set; } = null;

    // FIXME: This shouldn't be here
    public int DebitTo { get; set; }
    public int CreditTo { get; set; } // Is this even used
    //public required JournalType JournalType { get; set; }
}

// Immutable
public class PurchaseOrderLineItem : BaseEntity
{
    public static PurchaseOrderLineItem FromItem(int quantity, decimal discount, Item item)
    {
        return new PurchaseOrderLineItem
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

[JsonConverter(typeof(JsonStringEnumConverter<OrderStatus>))]
public enum OrderStatus : byte
{
    Closed = 0,
    Open = 1
}
