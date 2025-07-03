using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

namespace Api.Entities;

// Invoice-Trade
[JsonConverter(typeof(JsonStringEnumConverter<PaymentMethod>))]
public enum PaymentMethod
{
    Cash = 0x01,
    OnAccount = 0x02
}

// Transaction / Receipt
[Index(nameof(Number), IsUnique = true)]
public class Invoice : BaseEntity
{
    public int Number { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public required Customer Customer { get; set; }
    public ICollection<InvoiceLineItem> LineItems { get; set; } = [];
    public decimal VatableAmount { get; set; }
    public decimal VatAmount { get; set; }
    public decimal Discounted { get; set; }
    public decimal NetAmount { get; set; } // FIXME: Discount!
    public required JournalEntry JournalEntry { get; set; }

    public decimal? InstalmentBalance { get; set; }
    public ICollection<CollectionPayment> Payments { get; set; } = [];
}

public class InvoiceLineItem : BaseEntity
{
    public static InvoiceLineItem FromItem(int quantity, decimal discount, Item item)
    {
        return new InvoiceLineItem
        {
            OriginalItem = item,
            ItemSnapshot = new ItemSnapshot
            {
                Code = item.Code,
                Name = item.Name,
                UnitOfMeasurementId = item.Uom.Id,
                UnitOfMeasurement = item.Uom.Name,
                UnitPrice = item.UnitPrice, // Selling price
                UnitCost = item.UnitCost,
                //Discount = requestLineItem.Discount
            },
            Quantity = quantity,
            Discount = discount,
            LineTotal = (quantity * item.UnitPrice) /*- discount */
        };
    }
    public required Item OriginalItem { get; set; }
    public required ItemSnapshot ItemSnapshot { get; set; }
    public int Quantity { get; set; }
    public decimal Discount { get; set; }
    public decimal LineTotal { get; set; }
}