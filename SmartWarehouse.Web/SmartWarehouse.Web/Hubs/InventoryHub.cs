using Microsoft.AspNetCore.SignalR;

namespace SmartWarehouse.Web.Hubs;

public class InventoryHub : Hub
{
    public async Task NotifyItemUpdated(Guid itemId, int newQuantity)
    {
        // "Others" means everyone except the person who sent the message
        await Clients.Others.SendAsync("ReceiveItemUpdate", itemId, newQuantity);
    }
}