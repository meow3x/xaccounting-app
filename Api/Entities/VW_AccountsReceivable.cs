namespace Api.Entities;

public class VW_AccountsReceivable
{
    public required string CustomerId { get; set; }
    public required string Name { get; set; }
    public decimal? Debit { get; set; }
    public decimal? Credit { get; set; }
    public decimal? Receivable { get; set; }
}

public class VW_AccountsPayable
{
    public required string SupplierId { get; set; }
    public required string Name { get; set; }
    public decimal? Debit { get; set; }
    public decimal? Credit { get; set; }
    public decimal? Payable { get; set; }
}