using SmartWarehouse.Shared.Models;
using SmartWarehouse.Shared.Services;
using SQLite;

namespace SmartWarehouse.MAUI.Services;

public class LocalInventoryService: IInventoryDataService
{
    private SQLiteAsyncConnection _connection;

    public LocalInventoryService()
    {
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "warehouse.db3");
        _connection = new SQLiteAsyncConnection(dbPath);
        
        // Create table if it doesn't exist
        _ = _connection.CreateTableAsync<InventoryItem>();
    }
    public async Task<List<InventoryItem>> GetItemsAsync()
    {
        return await _connection.Table<InventoryItem>().ToListAsync();
    }

    public async Task<InventoryItem?> GetItemByIdAsync(Guid id)
    {
        return await _connection.Table<InventoryItem>().FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task SaveItemAsync(InventoryItem item)
    {
        if (item.Id == Guid.Empty)
        {
            item.Id = Guid.NewGuid();
            await _connection.InsertAsync(item);
        }
        var existingItem = await GetItemByIdAsync(item.Id);
        if (existingItem != null)
            await _connection.UpdateAsync(item);
        else
            await _connection.InsertAsync(item);
        
        // 💡 FUTURE STEP: Here we would add a flag "RequiresSync = true" 
        // to tell the background worker to push this to the server later.
    }
}