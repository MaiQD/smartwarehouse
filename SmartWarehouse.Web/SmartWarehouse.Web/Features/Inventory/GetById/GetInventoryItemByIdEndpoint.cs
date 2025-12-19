using FastEndpoints;
using SmartWarehouse.Shared.Models;
using SmartWarehouse.Web.Data;

namespace SmartWarehouse.Web.Features.Inventory.GetById;

public class GetInventoryItemByIdEndpoint(FakeInventoryDb db) : Endpoint<GetInventoryItemByIdRequest, InventoryItem>
{
    public override void Configure()
    {
        Get("api/inventory/{Id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetInventoryItemByIdRequest req, CancellationToken ct)
    {
        var item = await db.GetAsync(req.Id, ct);
        if (item == null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        await Send.OkAsync(item, ct);
    }
}

