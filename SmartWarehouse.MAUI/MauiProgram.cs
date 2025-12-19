using Microsoft.Extensions.Logging;
using SmartWarehouse.MAUI.Services;
using SmartWarehouse.Shared.Services;

namespace SmartWarehouse.MAUI;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts => { fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular"); });

        builder.Services.AddScoped<IBarcodeScanner, MobileBarcodeScanner>();
        builder.Services.AddScoped<IInventoryDataService, LocalInventoryService>();
        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}