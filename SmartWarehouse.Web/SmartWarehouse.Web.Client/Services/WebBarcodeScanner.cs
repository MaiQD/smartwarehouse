using Microsoft.JSInterop;
using SmartWarehouse.Shared.Services;

namespace SmartWarehouse.Web.Client.Services;

public class WebBarcodeScanner(IJSRuntime js): IBarcodeScanner
{
    public async Task<string> ScanAsync()
    {
        var result = await js.InvokeAsync<string>("prompt", "📷 (Web Simulation) Scan Barcode via Camera...");
        return result;
    }
}