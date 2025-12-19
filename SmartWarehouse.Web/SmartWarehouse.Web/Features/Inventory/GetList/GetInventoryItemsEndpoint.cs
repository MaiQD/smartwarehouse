using FastEndpoints;
using SmartWarehouse.Shared.Models;
using SmartWarehouse.Web.Data;

namespace SmartWarehouse.Web.Features.Inventory.GetList;

public class GetInventoryItemsEndpoint(FakeInventoryDb db) : EndpointWithoutRequest<List<InventoryItem>>
{
    public override void Configure()
    {
        Get("api/inventory");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var items = await db.GetAllAsync(ct);
        await Send.OkAsync(items.ToList(), ct);
    }
}

