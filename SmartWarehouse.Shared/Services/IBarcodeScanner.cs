namespace SmartWarehouse.Shared.Services;

public interface IBarcodeScanner
{
    Task<string> ScanAsync();
}