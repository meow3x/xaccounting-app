using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Api.Entities;

[Index(nameof(Code), IsUnique = true)]
public class Item : BaseEntity
{
    [MaxLength(25)]
    public required string Code { get; set; }
    [MaxLength(512)]
    public required string Name { get; set; }
    public required UnitOfMeasurement Uom { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal UnitCost { get; set; }
    public required ItemCategory Category { get; set; }
    public decimal Wholesale { get; set; }
    public decimal Reorder { get; set; }
}
