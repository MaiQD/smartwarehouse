using FastEndpoints;
using FastEndpoints.Swagger;
using SmartWarehouse.Web.Components;
using SmartWarehouse.Web.Hubs;
using SmartWarehouse.Shared.Services;
using SmartWarehouse.Web.Client.Services;
using SmartWarehouse.Web.Data;
using SmartWarehouse.Web.Features.Inventory.SaveItem;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri("http://localhost:5055") });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddFastEndpoints()
    .AddSwaggerDocument();

builder.Services.AddScoped<IBarcodeScanner, WebBarcodeScanner>();
builder.Services.AddScoped<IInventoryDataService, ApiInventoryService>();
builder.Services.AddSingleton<FakeInventoryDb>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.UseFastEndpoints()
    .UseSwaggerGen();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(
        typeof(SmartWarehouse.Web.Client._Imports).Assembly, 
        typeof(SmartWarehouse.Shared._Imports).Assembly); // Server assembly for static SSR

app.MapHub<InventoryHub>("/inventoryhub");

app.Run();