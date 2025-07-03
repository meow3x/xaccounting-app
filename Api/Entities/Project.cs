using Microsoft.EntityFrameworkCore;

namespace Api.Entities;

[Index(nameof(Number), IsUnique = true)]
public class Project : BaseEntity
{
    public required string Number { get; set; }
    public required string Name { get; set; }
}
