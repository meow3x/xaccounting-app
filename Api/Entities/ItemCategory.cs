using System.ComponentModel.DataAnnotations;

namespace Api.Entities;

public class ItemCategory : BaseEntity
{
    [MaxLength(255)]
    public required string Name { get; set; }
}