using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

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
    public decimal UnitPrice { get; set; }
    public decimal UnitCost { get; set; }
    //public decimal Discount { get; set; }
    //public decimal Total { get; set; }
}
