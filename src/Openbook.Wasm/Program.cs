using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using Openbook.Wasm;
using Openbook.Wasm.Pages;
using Openbook.Wasm.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:7071") });
builder.Services.AddScoped<AuthStateService>();
builder.Services.AddScoped<SignalRService>();

var host = builder.Build();

// Set JWT Authorization header from localStorage if present
var js = host.Services.GetRequiredService<IJSRuntime>();
var jwt = await js.InvokeAsync<string>("localStorage.getItem", "jwt");
if (!string.IsNullOrEmpty(jwt))
{
    var http = host.Services.GetRequiredService<HttpClient>();
    http.SetJwtAuthorizationHeader(jwt);
}

// Load AuthStateService user info
var authState = host.Services.GetRequiredService<AuthStateService>();
await authState.LoadAsync();

await host.RunAsync();
