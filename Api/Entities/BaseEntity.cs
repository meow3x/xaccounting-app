using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Entities;

public abstract class BaseEntity
{
    public virtual int Id { get; set; }

    [Column(TypeName = "timestamp without time zone")]
    public DateTime CreatedAt { get; init; } = DateTime.Now;
    [Column(TypeName = "timestamp without time zone")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
