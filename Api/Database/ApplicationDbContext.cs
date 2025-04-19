
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
    public DbSet<Project> Projects { get; set; }
    public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
    public DbSet<LineItem> LineItems { get; set; }
    public DbSet<JournalType> JournalTypes { get; set; }
    public DbSet<JournalEntry> JournalEntries { get; set; }
    public DbSet<JournalLine> JournalLines { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var epoch = new DateTime(year: 2025, month: 1, day: 1, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc);

        //var epoch = U ;
            
        //modelBuilder.Entity<BaseEntity>()
        //    .Property(e => e.CreatedAt)
        //    .HasDefaultValueSql("now()");

        modelBuilder.Entity<PurchaseOrder>()
            .HasOne<Account>()
            .WithMany()
            .HasForeignKey(po => po.DebitTo)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PurchaseOrder>()
            .HasOne<Account>()
            .WithMany()
            .HasForeignKey(po => po.CreditTo)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
        
        modelBuilder.Entity<PurchaseOrder>()
            .Property(po => po.Number)
            .UseIdentityAlwaysColumn()
            .HasIdentityOptions(startValue: 1000);

        modelBuilder.Entity<AccountType>()
            .HasData(
                new AccountType { Id = 1, Name = "Cash", CreatedAt = epoch, UpdatedAt = epoch },
                new AccountType { Id = 2, Name = "Bank", CreatedAt = epoch, UpdatedAt = epoch },
                new AccountType { Id = 3, Name = "Trade Receivable", CreatedAt = epoch, UpdatedAt = epoch },
                new AccountType { Id = 4, Name = "Non-Trade Receivable", CreatedAt = epoch, UpdatedAt = epoch },
                new AccountType { Id = 5, Name = "Material", CreatedAt = epoch, UpdatedAt = epoch },
                new AccountType { Id = 6, Name = "Inventory", CreatedAt = epoch, UpdatedAt = epoch },
                new AccountType { Id = 7, Name = "Properties", CreatedAt = epoch, UpdatedAt = epoch },
                new AccountType { Id = 8, Name = "Accumulated Depreciation", CreatedAt = epoch, UpdatedAt = epoch },
                new AccountType { Id = 9, Name = "Other Current Assets", CreatedAt = epoch, UpdatedAt = epoch },
                new AccountType { Id = 10, Name = "Other Non-Current Assets", CreatedAt = epoch, UpdatedAt = epoch },
                new AccountType { Id = 11, Name = "Payable", CreatedAt = epoch, UpdatedAt = epoch },
                new AccountType { Id = 12, Name = "Other Current Liabilities", CreatedAt = epoch, UpdatedAt = epoch },
                new AccountType { Id = 13, Name = "Other Non-Current Liabilities", CreatedAt = epoch, UpdatedAt = epoch },
                new AccountType { Id = 14, Name = "Capital", CreatedAt = epoch, UpdatedAt = epoch },
                new AccountType { Id = 15, Name = "Sales", CreatedAt = epoch, UpdatedAt = epoch },
                new AccountType { Id = 16, Name = "Sales Discount", CreatedAt = epoch, UpdatedAt = epoch },
                new AccountType { Id = 17, Name = "Expenses", CreatedAt = epoch, UpdatedAt = epoch }
            );

        modelBuilder.Entity<UnitOfMeasurement>()
            .HasData(
                new UnitOfMeasurement { Id = 1, Name = "bag", CreatedAt = epoch, UpdatedAt = epoch },
                new UnitOfMeasurement { Id = 2, Name = "bags", CreatedAt = epoch, UpdatedAt = epoch },
                new UnitOfMeasurement { Id = 3, Name = "bottle", CreatedAt = epoch, UpdatedAt = epoch },
                new UnitOfMeasurement { Id = 4, Name = "box", CreatedAt = epoch, UpdatedAt = epoch },
                new UnitOfMeasurement { Id = 5, Name = "can", CreatedAt = epoch, UpdatedAt = epoch },
                new UnitOfMeasurement { Id = 6, Name = "cu", CreatedAt = epoch, UpdatedAt = epoch },
                new UnitOfMeasurement { Id = 7, Name = "cubic meter", CreatedAt = epoch, UpdatedAt = epoch },
                new UnitOfMeasurement { Id = 8, Name = "dumptruck", CreatedAt = epoch, UpdatedAt = epoch },
                new UnitOfMeasurement { Id = 9, Name = "elf", CreatedAt = epoch, UpdatedAt = epoch },
                new UnitOfMeasurement { Id = 10, Name = "gallon", CreatedAt = epoch, UpdatedAt = epoch },
                new UnitOfMeasurement { Id = 11, Name = "half elf", CreatedAt = epoch, UpdatedAt = epoch },
                new UnitOfMeasurement { Id = 12, Name = "kilo", CreatedAt = epoch, UpdatedAt = epoch },
                new UnitOfMeasurement { Id = 13, Name = "liter", CreatedAt = epoch, UpdatedAt = epoch },
                new UnitOfMeasurement { Id = 14, Name = "meter", CreatedAt = epoch, UpdatedAt = epoch },
                new UnitOfMeasurement { Id = 15, Name = "pad", CreatedAt = epoch, UpdatedAt = epoch },
                new UnitOfMeasurement { Id = 16, Name = "pail", CreatedAt = epoch, UpdatedAt = epoch },
                new UnitOfMeasurement { Id = 17, Name = "pair", CreatedAt = epoch, UpdatedAt = epoch },
                new UnitOfMeasurement { Id = 18, Name = "piece", CreatedAt = epoch, UpdatedAt = epoch },
                new UnitOfMeasurement { Id = 19, Name = "roll", CreatedAt = epoch, UpdatedAt = epoch },
                new UnitOfMeasurement { Id = 20, Name = "sack", CreatedAt = epoch, UpdatedAt = epoch },
                new UnitOfMeasurement { Id = 21, Name = "set", CreatedAt = epoch, UpdatedAt = epoch },
                new UnitOfMeasurement { Id = 22, Name = "tin", CreatedAt = epoch, UpdatedAt = epoch },
                new UnitOfMeasurement { Id = 23, Name = "tube", CreatedAt = epoch, UpdatedAt = epoch },
                new UnitOfMeasurement { Id = 24, Name = "unit", CreatedAt = epoch, UpdatedAt = epoch }
            );

        modelBuilder.Entity<ItemCategory>()
            .HasData(
                new ItemCategory { Id = 1, Name = "Electrical", CreatedAt = epoch, UpdatedAt = epoch },
                new ItemCategory { Id = 2, Name = "Office Equipment", CreatedAt = epoch, UpdatedAt = epoch },
                new ItemCategory { Id = 3, Name = "Tools and Equipment", CreatedAt = epoch, UpdatedAt = epoch },
                new ItemCategory { Id = 4, Name = "Motorpool", CreatedAt = epoch, UpdatedAt = epoch },
                new ItemCategory { Id = 5, Name = "Plumbing", CreatedAt = epoch, UpdatedAt = epoch },
                new ItemCategory { Id = 6, Name = "Finishing & Paintings", CreatedAt = epoch, UpdatedAt = epoch },
                new ItemCategory { Id = 7, Name = "Masonry", CreatedAt = epoch, UpdatedAt = epoch },
                new ItemCategory { Id = 8, Name = "Metals", CreatedAt = epoch, UpdatedAt = epoch },
                new ItemCategory { Id = 9, Name = "Woods & Plastics", CreatedAt = epoch, UpdatedAt = epoch },
                new ItemCategory { Id = 10, Name = "Consumables", CreatedAt = epoch, UpdatedAt = epoch },
                new ItemCategory { Id = 11, Name = "Doors & Windows", CreatedAt = epoch, UpdatedAt = epoch },
                new ItemCategory { Id = 12, Name = "Office Supplies", CreatedAt = epoch, UpdatedAt = epoch },
                new ItemCategory { Id = 13, Name = "Furniture & Fixture", CreatedAt = epoch, UpdatedAt = epoch },
                new ItemCategory { Id = 14, Name = "Services", CreatedAt = epoch, UpdatedAt = epoch }
            );

        modelBuilder.Entity<PaymentTerm>()
            .HasData(
                new PaymentTerm { Id = 1, Label = "Cash", CreatedAt = epoch, UpdatedAt = epoch },
                new PaymentTerm { Id = 2, Label = "7 days", CreatedAt = epoch, UpdatedAt = epoch },
                new PaymentTerm { Id = 3, Label = "30 Days", CreatedAt = epoch, UpdatedAt = epoch },
                new PaymentTerm { Id = 4, Label = "120 Day", CreatedAt = epoch, UpdatedAt = epoch },
                new PaymentTerm { Id = 5, Label = "COD", CreatedAt = epoch, UpdatedAt = epoch }
            );

        modelBuilder.Entity<JournalType>()
            .HasData(
                new JournalType { Id = 1, Name = "General", CreatedAt = epoch, UpdatedAt = epoch },
                new JournalType { Id = 2, Name = "Disbursements", CreatedAt = epoch, UpdatedAt = epoch },
                new JournalType { Id = 3, Name = "Payables", CreatedAt = epoch, UpdatedAt = epoch },
                new JournalType { Id = 4, Name = "Receipts", CreatedAt = epoch, UpdatedAt = epoch },
                new JournalType { Id = 5, Name = "Purchases", CreatedAt = epoch, UpdatedAt = epoch },
                new JournalType { Id = 6, Name = "Sales", CreatedAt = epoch, UpdatedAt = epoch },
                new JournalType { Id = 7, Name = "Production", CreatedAt = epoch, UpdatedAt = epoch },
                new JournalType { Id = 8, Name = "Cancelled", CreatedAt = epoch, UpdatedAt = epoch }
            );

        modelBuilder.Entity<Project>()
            .HasData(
                new Project { Id = 1, Number = "DEFP001", Name = "Default Project", CreatedAt = epoch, UpdatedAt = epoch }
            );
    }
}
