using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using newFrontend.Client.Services;
using System.Globalization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddMudServices();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:8000") });

builder.Services.AddScoped<IParkingService, ParkingService>();
builder.Services.AddScoped<IPricesService, PricesService>();

CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pt-BR");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("pt-BR");

await builder.Build().RunAsync();
