namespace Play.Inventory.Service.Dtos;

public record GrantItemDto(Guid userId, Guid catalogItemId, int quantity);

public record InventoryItemDto(Guid catalogItemId, int quantity, DateTimeOffset acquiredDate);
