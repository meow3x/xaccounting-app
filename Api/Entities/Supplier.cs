using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Api.Entities;

[Index(nameof(SupplierId), IsUnique = true)]
public class Supplier : BaseEntity
{
    [MaxLength(255)]
    public required string SupplierId { get; set; }
    public required string Name { get; set; }
    public required Address Address { get; set; }
    public string? Tin { get; set; }
    public decimal? Discount { get; set; }
    public decimal? CreditLimit { get; set; }
    public PaymentTerm? PaymentTerm { get; set; }
}