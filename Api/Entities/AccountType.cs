using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Entities;

public class AccountType : BaseEntity
{
    public string? Label { get; set; }
}
