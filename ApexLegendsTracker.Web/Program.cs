using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using ApexLegendsTracker.Web;
using ApexLegendsTracker.Web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

string apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5165/";

builder.Services.AddScoped(_ => new HttpClient { BaseAddress = new Uri(apiBaseUrl) });
builder.Services.AddScoped<IApexTrackerApiClient, ApexTrackerApiClient>();
builder.Services.AddScoped<PlayerLookupState>();

WebAssemblyHost host = builder.Build();

string? appInsightsConnectionString = host.Configuration["APEXWEB_APPINSIGHTS_CONNECTION_STRING"]
	?? Environment.GetEnvironmentVariable("APEXWEB_APPINSIGHTS_CONNECTION_STRING");

if (!string.IsNullOrWhiteSpace(appInsightsConnectionString))
{
	IJSRuntime jsRuntime = host.Services.GetRequiredService<IJSRuntime>();
	await jsRuntime.InvokeVoidAsync("apexTelemetry.init", appInsightsConnectionString);
}

await host.RunAsync();
