using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using StudiBloc3_Mercadona.App.Components;
using StudiBloc3_Mercadona.App.Services;
using Syncfusion.Blazor;
using Syncfusion.Licensing;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NHaF5cWWBCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdgWH5ed3RXRGRYVUF1X0A=");
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSyncfusionBlazor();

builder.Services.AddScoped<AuthenticationService>();

builder.Services.AddScoped(sp => new HttpClient {BaseAddress = new Uri("https://localhost:7173")});
builder.Services.AddScoped<ApiProductService>();
builder.Services.AddScoped<ApiCategoryService>();
builder.Services.AddScoped<ApiPromotionService>();
builder.Services.AddScoped<ApiProductPromotionService>();

await builder.Build().RunAsync();