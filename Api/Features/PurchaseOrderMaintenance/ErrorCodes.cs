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

}
