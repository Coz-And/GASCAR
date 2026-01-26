using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using GASCAR.Web;
using GASCAR.Web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// 🔌 Configura HttpClient con timeout
var httpClientHandler = new HttpClientHandler();
var httpClient = new HttpClient(httpClientHandler)
{
    BaseAddress = new Uri("http://localhost:5184/"),
    Timeout = TimeSpan.FromSeconds(30)
};

builder.Services.AddScoped(sp => httpClient);
builder.Services.AddScoped<ApiService>();
builder.Services.AddScoped<AuthStateService>();

await builder.Build().RunAsync();
