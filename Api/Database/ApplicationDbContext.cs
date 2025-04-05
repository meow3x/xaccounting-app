
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
    public DbSet<Item> Items { get; set; }
    public DbSet<UnitOfMeasurement> UnitOfMeasurements { get; set; }
    public DbSet<ItemCategory> ItemCategories { get; set; }
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<PaymentTerm> PaymentTerms { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Employee> Employees { get; set; }

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

        modelBuilder.Entity<UnitOfMeasurement>(b =>
        {
            b.HasData(
                new UnitOfMeasurement { Id = 1, Name = "bag" },
                new UnitOfMeasurement { Id = 2, Name = "bags" },
                new UnitOfMeasurement { Id = 3, Name = "bottle" },
                new UnitOfMeasurement { Id = 4, Name = "box" },
                new UnitOfMeasurement { Id = 5, Name = "can" },
                new UnitOfMeasurement { Id = 6, Name = "cu" },
                new UnitOfMeasurement { Id = 7, Name = "cubic meter" },
                new UnitOfMeasurement { Id = 8, Name = "dumptruck" },
                new UnitOfMeasurement { Id = 9, Name = "elf" },
                new UnitOfMeasurement { Id = 10, Name = "gallon" },
                new UnitOfMeasurement { Id = 11, Name = "half elf" },
                new UnitOfMeasurement { Id = 12, Name = "kilo" },
                new UnitOfMeasurement { Id = 13, Name = "liter" },
                new UnitOfMeasurement { Id = 14, Name = "meter" },
                new UnitOfMeasurement { Id = 15, Name = "pad" },
                new UnitOfMeasurement { Id = 16, Name = "pail" },
                new UnitOfMeasurement { Id = 17, Name = "pair" },
                new UnitOfMeasurement { Id = 18, Name = "piece" },
                new UnitOfMeasurement { Id = 19, Name = "roll" },
                new UnitOfMeasurement { Id = 20, Name = "sack" },
                new UnitOfMeasurement { Id = 21, Name = "set" },
                new UnitOfMeasurement { Id = 22, Name = "tin" },
                new UnitOfMeasurement { Id = 23, Name = "tube" },
                new UnitOfMeasurement { Id = 24, Name = "unit" }
            );
        });

        modelBuilder.Entity<ItemCategory>(b =>
        {
            b.HasData(
                new ItemCategory { Id = 1, Name = "Electrical" },
                new ItemCategory { Id = 2, Name = "Office Equipment" },
                new ItemCategory { Id = 3, Name = "Tools and Equipment" },
                new ItemCategory { Id = 4, Name = "Motorpool" },
                new ItemCategory { Id = 5, Name = "Plumbing" },
                new ItemCategory { Id = 6, Name = "Finishing & Paintings" },
                new ItemCategory { Id = 7, Name = "Masonry" },
                new ItemCategory { Id = 8, Name = "Metals" },
                new ItemCategory { Id = 9, Name = "Woods & Plastics" },
                new ItemCategory { Id = 10, Name = "Consumables" },
                new ItemCategory { Id = 11, Name = "Doors & Windows" },
                new ItemCategory { Id = 12, Name = "Office Supplies" },
                new ItemCategory { Id = 13, Name = "Furniture & Fixture" },
                new ItemCategory { Id = 14, Name = "Services" }
            );
        });

        modelBuilder.Entity<PaymentTerm>(b =>
        {
            b.HasData(
                new PaymentTerm { Id = 1, Label = "Cash" },
                new PaymentTerm { Id = 2, Label = "7 days" },
                new PaymentTerm { Id = 3, Label = "30 Days" },
                new PaymentTerm { Id = 4, Label = "120 Day" },
                new PaymentTerm { Id = 5, Label = "COD"  }
            );
        });
    }
}
