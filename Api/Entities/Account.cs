using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Entities;

[Index(nameof(AccountId), IsUnique = true)]
public class Account : BaseEntity
{
    // Fixme: Primary key should be [OrganizationId, Id]
    [Column(TypeName = "varchar(200)")]
    public required string AccountId { get; set; }
    public required string Name { get; set; }
    public required AccountType AccountType { get; set; }
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
    public decimal Balance { get; set;  }
    public decimal YearEndBudget { get; set; }
}
