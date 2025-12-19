using System.Net.Http.Json;
using SmartWarehouse.Shared.Models;
using SmartWarehouse.Shared.Services;

namespace SmartWarehouse.Web.Client.Services;

public class ApiInventoryService(HttpClient httpClient) : IInventoryDataService
{
    public async Task<List<InventoryItem>> GetItemsAsync()
    {
        return await httpClient.GetFromJsonAsync<List<InventoryItem>>("api/inventory") ?? [];
    }

    public async Task<InventoryItem?> GetItemByIdAsync(Guid id)
    {
        // Simple API call
        return await httpClient.GetFromJsonAsync<InventoryItem>($"api/inventory/{id}");
    }

    public async Task SaveItemAsync(InventoryItem item)
    {
        await httpClient.PostAsJsonAsync("api/inventory", item);
    }
}