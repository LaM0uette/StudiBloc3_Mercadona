using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using StudiBloc3_Mercadona.App.Components;
using StudiBloc3_Mercadona.App.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddScoped(sp => new HttpClient {BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)});

builder.Services.AddScoped(sp => new HttpClient {BaseAddress = new Uri("https://localhost:7173")});
builder.Services.AddScoped<ApiProductService>();

await builder.Build().RunAsync();