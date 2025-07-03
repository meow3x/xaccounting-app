namespace Api.Features.InventoryMaintenance;

public record InventoryByQuantityDto(
    string ItemCode,
    string ItemName,
    int Quantity
);