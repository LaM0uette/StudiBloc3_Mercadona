using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using StudiBloc3_Mercadona.WebApp.Components;
using StudiBloc3_Mercadona.WebApp.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient {BaseAddress = new Uri("https://localhost:7173")});
builder.Services.AddScoped<ApiProductService>();
builder.Services.AddScoped<ApiPromotionService>();
builder.Services.AddScoped<ApiProductPromotionService>();

await builder.Build().RunAsync();
