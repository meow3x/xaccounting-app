namespace Api.Entities;

public class JournalType : BaseEntity
{
    public required string Name { get; set; }
}

public class JournalEntry : BaseEntity
{
    public required JournalType JournalType { get; set; }
    public required string Description { get; set; }
    public ICollection<JournalLine> Lines { get; set; } = [];
    public string? ReferenceNumber1 { get; set; }
}

public class JournalLine : BaseEntity
{
    public int JournalEntryId { get; set; }
    public int LineNumber { get; set; }
    public required string Description { get; set; }
    public required Account Account { get; set; }
    
    // Debit and Credit are mutually exclusive
    public decimal? Debit { get; set; }
    public decimal? Credit { get; set; }
    
    public string? ReferenceNumber1 { get; set; }
}