
using Api.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Database;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<AccountType> AccountTypes { get; set; }
    public DbSet<Account>  Accounts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountType>(b =>
        {
            b.HasData(
                new AccountType { Id = 1, Label = "Cash" },
                new AccountType { Id = 2, Label = "Bank" },
                new AccountType { Id = 3, Label = "Trade Receivable" },
                new AccountType { Id = 4, Label = "Non-Trade Receivable" },
                new AccountType { Id = 5, Label = "Material" },
                new AccountType { Id = 6, Label = "Inventory" },
                new AccountType { Id = 7, Label = "Properties" },
                new AccountType { Id = 8, Label = "Accumulated Depreciation" },
                new AccountType { Id = 9, Label = "Other Current Assets" },
                new AccountType { Id = 10, Label = "Other Non-Current Assets" },
                new AccountType { Id = 11, Label = "Payable" },
                new AccountType { Id = 12, Label = "Other Current Liabilities" },
                new AccountType { Id = 13, Label = "Other Non-Current Liabilities" },
                new AccountType { Id = 14, Label = "Capital" },
                new AccountType { Id = 15, Label = "Sales" },
                new AccountType { Id = 16, Label = "Sales Discount" },
                new AccountType { Id = 17, Label = "Expenses" }
            );
        });
    }
}
