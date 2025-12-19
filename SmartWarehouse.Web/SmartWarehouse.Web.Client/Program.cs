using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SmartWarehouse.Shared.Services;
using SmartWarehouse.Web.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<IBarcodeScanner, WebBarcodeScanner>();
builder.Services.AddScoped<IInventoryDataService, ApiInventoryService>();
await builder.Build().RunAsync();