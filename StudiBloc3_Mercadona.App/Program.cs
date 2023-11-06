using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using StudiBloc3_Mercadona.App.Components;
using StudiBloc3_Mercadona.App.Services;
using Syncfusion.Blazor;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSyncfusionBlazor();

builder.Services.AddScoped(sp => new HttpClient {BaseAddress = new Uri("https://localhost:7173")});
builder.Services.AddScoped<ApiProductService>();
builder.Services.AddScoped<ApiCategoryService>();

await builder.Build().RunAsync();