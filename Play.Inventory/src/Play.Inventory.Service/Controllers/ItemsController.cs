using Microsoft.AspNetCore.Mvc;
using Play.Common;
using Play.Inventory.Service.Dtos;
using Play.Inventory.Service.Entities;

namespace Play.Inventory.Service.Controllers;

[ApiController]
[Route("items")]
public class ItemsController : ControllerBase
{
  private readonly IRepository<InventoryItem> itemsRepository;

  public ItemsController(IRepository<InventoryItem> itemsRepository)
  {
    this.itemsRepository = itemsRepository;
  }

  [HttpGet]
  public async Task<ActionResult<IEnumerable<InventoryItemDto>>> GetAsync(Guid userId)
  {
    if (userId == Guid.Empty)
    {
      return BadRequest();
    }

    var items = (await itemsRepository.GetAllAsync(item => item.UserId == userId)).Select(item => item.AsDto());

    return Ok(items);
  }

  [HttpPost]
  public async Task<ActionResult> PostAsync(GrantItemDto grantItemDto)
  {
    var inventoryItem = await itemsRepository.GetAsync(item => item.UserId == grantItemDto.userId && item.CatalogItemId == grantItemDto.catalogItemId);

    if (inventoryItem == null)
    {
      inventoryItem = new InventoryItem
      {
        CatalogItemId = grantItemDto.catalogItemId,
        UserId = grantItemDto.userId,
        Quantity = grantItemDto.quantity,
        AcquiredDate = DateTimeOffset.UtcNow
      };

      await itemsRepository.CreateAsync(inventoryItem);
    }
    else
    {
      inventoryItem.Quantity += grantItemDto.quantity;
      await itemsRepository.UpdateAsync(inventoryItem);
    }

    return Ok();
  }
}