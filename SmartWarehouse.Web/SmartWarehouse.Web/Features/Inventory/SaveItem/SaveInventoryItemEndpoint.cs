using FastEndpoints;
using Microsoft.AspNetCore.SignalR;
using SmartWarehouse.Shared.Models;
using SmartWarehouse.Web.Data;
using SmartWarehouse.Web.Hubs;

namespace SmartWarehouse.Web.Features.Inventory.SaveItem;

public class SaveInventoryItemEndpoint(IHubContext<InventoryHub> hubContext, FakeInventoryDb db) : Endpoint<InventoryItem, InventoryItem>
{
    private IHubContext<InventoryHub> _hubContext { get; } = hubContext;

    public override void Configure()
    {
        Post("api/inventory");
        AllowAnonymous();
    }

    public override async Task HandleAsync(InventoryItem req, CancellationToken ct)
    {
        var existing = req.Id == Guid.Empty ? null : await db.GetAsync(req.Id, ct);
        if (existing != null)
        {
            existing.Quantity = req.Quantity;
            existing.Name = req.Name;
            existing.Sku = req.Sku;
            await db.UpsertAsync(existing, ct);
            req = existing; // ensure the response contains the stored object
        }
        else
        {
            var stored = await db.UpsertAsync(req, ct);
            req = stored;
        }

        await _hubContext.Clients.All.SendAsync("ReceiveItemUpdate", req.Id, req.Quantity, ct);
        await Send.OkAsync(req, ct);
    }
}