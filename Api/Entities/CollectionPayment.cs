using Microsoft.EntityFrameworkCore;
using NodaTime;

namespace Api.Entities;

[Index(nameof(ReceiptNumber), IsUnique = true)]
public class CollectionPayment : BaseEntity
{
    public int SequenceNumber { get; set; }
    public int ReceiptNumber { get; set; }
    //public required Invoice SourceTransaction { get; set; }
    public string? ReferenceNumber { get; set; }
    public decimal Amount { get; set; }
    public required JournalEntry JournalEntry { get; set; }
}
