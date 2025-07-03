namespace Api.Features.PurchaseOrderMaintenance;

public static class ErrorCodes
{
    public static readonly string E_NON_DISTINCT_LINE_ITEMS = "E_PO_001";
    public static readonly string E_ITEM_NOT_FOUND = "E_PO_002";
    public static readonly string E_PROJECT_NOT_FOUND = "E_PO_003";
    public static readonly string E_SUPPLIER_NOT_FOUND = "E_PO_004";
    public static readonly string E_INVALID_ACCOUNT = "E_PO_005";
    public static readonly string E_IDENTICAL_DEBIT_CREDIT_TARGET = "E_PO_006";
    public static readonly string E_ORDER_NOT_FOUND = "E_PO_007";
    public static readonly string E_ORDER_NOT_OPEN = "E_PO_008";
    public static readonly string E_COST_CENTER_NOT_FOUND = "E_AP_001";
    public static readonly string E_PAYEE_NOT_FOUND = "E_DB_001";
    public static readonly string E_VOUCHER_NOT_FOUND = "E_DB_002";
    public static readonly string E_INVALID_CUSTOMER = "E_INV_001";
}

public sealed class AppErrorCode
{
    public string Code { get; }
    public string Description { get; }

    private AppErrorCode(string code, string description)
    {
        Code = code;
        Description = description;
    }

    public override string ToString() => Code;

    public static readonly AppErrorCode EntityNotFound = new("E_NOT_FOUND", "Entity not found");
    public static readonly AppErrorCode LineItemElementNotFound = new("E_ITEM_NOT_FOUND", "E_PO_002");
    public static readonly AppErrorCode NotAnInstalment = new("E_NOT_INSTALMENT", "E_INV_002");
    public static readonly AppErrorCode LineItemInvalidUnitPrice = new("E_LINEITEM_NO_UNIT_PRICE", "E_IN_002");
    public static readonly AppErrorCode E_NON_DISTINCT_LINE_ITEMS = new("E_NON_DISTINCT_LINE_ITEMS", "E_PO_001");
    public static readonly AppErrorCode E_PROJECT_NOT_FOUND = new("E_PROJECT_NOT_FOUND", "E_PO_003");
    public static readonly AppErrorCode E_SUPPLIER_NOT_FOUND = new("E_SUPPLIER_NOT_FOUND", "E_PO_004");
    public static readonly AppErrorCode E_INVALID_ACCOUNT = new("E_INVALID_ACCOUNT", "E_PO_005");
    public static readonly AppErrorCode IdenticalDebitAndCreditAccount = new("E_IDENTICAL_DEBIT_CREDIT_TARGET", "E_PO_006");
    public static readonly AppErrorCode E_ORDER_NOT_FOUND = new("E_ORDER_NOT_FOUND", "E_PO_007");
    public static readonly AppErrorCode E_ORDER_NOT_OPEN = new("E_ORDER_NOT_OPEN", "E_PO_008");
    public static readonly AppErrorCode E_COST_CENTER_NOT_FOUND = new("E_COST_CENTER_NOT_FOUND", "E_AP_001");
    public static readonly AppErrorCode E_PAYEE_NOT_FOUND = new("E_PAYEE_NOT_FOUND", "E_DB_001");
    public static readonly AppErrorCode E_VOUCHER_NOT_FOUND = new("E_VOUCHER_NOT_FOUND", "E_DB_002");
    public static readonly AppErrorCode E_INVALID_CUSTOMER = new("E_INVALID_CUSTOMER", "E_IN_001");
}