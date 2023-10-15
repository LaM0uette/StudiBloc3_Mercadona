using Microsoft.AspNetCore.Components;
using StudiBloc3_Mercadona.Model;
using StudiBloc3_Mercadona.WebApp.Services;

namespace StudiBloc3_Mercadona.WebApp.Components.Pages;

public class CatalogBase : ComponentBase
{
    #region Statements

    protected IEnumerable<Product> Products { get; private set; } = new List<Product>();
    
    [Inject] private ApiProductService ApiProductService { get; set; } = default!;

    #endregion

    #region Functions

    protected override async Task OnInitializedAsync()
    {
        await LoadProducts();
    }
    
    private async Task LoadProducts()
    {
        Products = await ApiProductService.GetAllProductsAsync();
    }

    #endregion
}