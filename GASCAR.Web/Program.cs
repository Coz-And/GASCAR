using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using GASCAR.Web;
using GASCAR.Web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// 🔌 API URL — DEVE ESSERE 5184
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("http://localhost:5184/")
});

builder.Services.AddScoped<ApiService>();
builder.Services.AddScoped<AuthStateService>();

await builder.Build().RunAsync();
