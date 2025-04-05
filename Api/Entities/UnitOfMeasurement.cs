using System.ComponentModel.DataAnnotations;

namespace Api.Entities
{
    public class UnitOfMeasurement : BaseEntity
    {
        [MaxLength(255)]
        public required string Name { get; set; }
    }
}