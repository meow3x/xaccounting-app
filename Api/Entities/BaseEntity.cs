using NodaTime;
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

    [Column(TypeName = "timestamp with time zone")]
    public Instant CreatedAt { get; init; } = SystemClock.Instance.GetCurrentInstant();

    [Column(TypeName = "timestamp with time zone")]
    public Instant UpdatedAt { get; set; } = SystemClock.Instance.GetCurrentInstant();
}
