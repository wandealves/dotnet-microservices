namespace Play.Inventory.Service.Dtos;

public record GrantItemDto(Guid userId, Guid catalogItemId, int quantity);

public record InventoryItemDto(Guid catalogItemId, string name, string description, int quantity, DateTimeOffset acquiredDate);
public record CatalogItemDto(Guid Id, string Name, string Description);

