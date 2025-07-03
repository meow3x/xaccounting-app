using Api.Entities;

namespace Api.Features.AccountsPayableMaintenance.Query;


public record SupplierPayableBalance(
    Supplier Supplier,
    decimal Balance
);
