using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
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
    
    protected Product product = new();

    protected async Task HandleValidSubmit()
    {
        await ApiProductService.AddProductAsync(product);
    }

    protected async Task HandleFileSelect(InputFileChangeEventArgs e)
    {
        var imageFiles = e.GetMultipleFiles();
        var imageFile = imageFiles.FirstOrDefault();
        if (imageFile != null)
        {
            await using var stream = imageFile.OpenReadStream(maxAllowedSize: 1024 * 1024);
            var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            product.Image = ms.ToArray();
        }
    }
}