using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
   
    /*
     * Source of journal entry:
     *  - Accounts payable, Disbursement, etc..
     */
    //public string? SourceEntityType { get; set; }
    //public int? SourceEntityId { get; set; } 

    // FIXME: Combine these into a single column since they are mutually exclusive
    //public int? AccountsPayableId { get; set; }
    //public int? PaymentId { get; set; }
}

public class JournalLine : BaseEntity
{
    [ForeignKey(nameof(JournalEntry))]
    public int JournalEntryId { get; set; }
    public int LineNumber { get; set; }
    public string? Description { get; set; }
    public required Account Account { get; set; }
    
    // Debit and Credit are mutually exclusive
    public decimal? Debit { get; set; }
    public decimal? Credit { get; set; }
    public CostCenter? CostCenter { get; set; }
    /*
     * AP (from PO)
     *   debit - PO number
     *   credit - XX
     * AP (manual)
     *   debit - APV number
     *   credit - APV number
     * Disbursement
     *   debit - 
     *   
     */
    public string? ReferenceNumber1 { get; set; }
}