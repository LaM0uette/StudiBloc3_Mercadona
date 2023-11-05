using Microsoft.AspNetCore.Components;
using StudiBloc3_Mercadona.Model;
using StudiBloc3_Mercadona.ServerApp.Services;

namespace StudiBloc3_Mercadona.ServerApp.Components.Pages;

public class HomeBase : ComponentBase
{
    #region Statements
    
    [Inject] private ApiProductService ApiProductService { get; set; } = default!;

    protected List<Product> Products { get; set; } = new();
    
    protected override async Task OnInitializedAsync()
    {
        Products = await ApiProductService.GetAllProductsAsync();
    }

    #endregion

    #region Functions

    protected static string ImageDataUrl(byte[] imageBytes)
    {
        var base64String = Convert.ToBase64String(imageBytes);
        return $"data:image/jpeg;base64,{base64String}";
    }

    #endregion
}