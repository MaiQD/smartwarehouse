using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SmartWarehouse.Shared.Services;
using SmartWarehouse.Web.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddScoped<IBarcodeScanner, WebBarcodeScanner>();
await builder.Build().RunAsync();