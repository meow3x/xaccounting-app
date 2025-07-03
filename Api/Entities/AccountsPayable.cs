using Microsoft.EntityFrameworkCore;
using NodaTime;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Api.Entities;

// TODO: Receiving purchase order must create accounts payable
[Index(nameof(VoucherNumber), IsUnique = true)]
public class AccountsPayable : BaseEntity
{
    // Auto-generated
    public int VoucherNumber { get; set; }
    // To-be printed on the voucher form
    public required string ReferenceNumber { get; set; }
    public required Supplier Supplier { get; set; }
    //public required string Description { get; set; } // Invoice/PO/etc...\
    public required JournalEntry JournalEntry { get; set; }

    [Column(TypeName = "date")]
    public LocalDate? DueDate { get; set; }
    public int? Terms { get; set; }

    public decimal TotalAmount { get; set; }
    public decimal Balance { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter<ChequeStatus>))]
public enum ChequeStatus
{
    Pending,
    Printed
}

// Disbursement
[Index(nameof(VoucherNumber), IsUnique = true)]
public class Payment : BaseEntity
{
    public int VoucherNumber { get; set; } // check voucher - auto generated
    public required JournalEntry JournalEntry { get; set; }
    public required string ReferenceNumber { get; set; } // Check number / etc..
    public required Supplier Payee { get; set; }

    public int? ApvNumber { get; set; } // A/P voucher number

    public bool IsCheque { get; set; }
    //public int? CheckNumber { get; set; }
    public ChequeStatus? ChequeStatus { get; set; }

    public decimal TotalAmount { get; set; }
}