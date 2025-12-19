using System.Collections.Concurrent;
using SmartWarehouse.Shared.Models;

namespace SmartWarehouse.Web.Data;

/// <summary>
/// A tiny, thread-safe in-memory "fake" database for InventoryItem used in development and tests.
/// Methods are async to mimic a real DB surface and to make it easy to swap for a real repository later.
/// </summary>
public class FakeInventoryDb
{
    private readonly ConcurrentDictionary<Guid, InventoryItem> _items = new();

    public Task<InventoryItem?> GetAsync(Guid id, CancellationToken ct = default)
    {
        _items.TryGetValue(id, out var item);
        return Task.FromResult(item);
    }

    public Task<IEnumerable<InventoryItem>> GetAllAsync(CancellationToken ct = default)
    {
        return Task.FromResult(_items.Values.AsEnumerable());
    }

    public Task<InventoryItem> UpsertAsync(InventoryItem item, CancellationToken ct = default)
    {
        if (item.Id == Guid.Empty) item.Id = Guid.NewGuid();
        // create a shallow copy to avoid external modifications after storing
        var stored = new InventoryItem
        {
            Id = item.Id,
            Name = item.Name,
            Quantity = item.Quantity,
            Sku = item.Sku
        };

        _items.AddOrUpdate(stored.Id, stored, (_, _) => stored);
        return Task.FromResult(stored);
    }

    public Task<bool> RemoveAsync(Guid id, CancellationToken ct = default)
    {
        return Task.FromResult(_items.TryRemove(id, out _));
    }

    public void Clear() => _items.Clear();
}
