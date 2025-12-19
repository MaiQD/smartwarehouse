using SmartWarehouse.Shared.Models;

namespace SmartWarehouse.Shared.Services;

public interface IInventoryDataService
{
    Task<List<InventoryItem>> GetItemsAsync();
    Task<InventoryItem?> GetItemByIdAsync(Guid id);
    Task SaveItemAsync(InventoryItem item);
}