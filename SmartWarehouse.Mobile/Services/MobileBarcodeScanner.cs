using SmartWarehouse.Shared.Services;

namespace SmartWarehouse.Mobile.Services;

public class MobileBarcodeScanner: IBarcodeScanner
{
    public async Task<string> ScanAsync()
    {
        await Task.Delay(2000);
        return "SKU-MOBILE-999";
    }
}