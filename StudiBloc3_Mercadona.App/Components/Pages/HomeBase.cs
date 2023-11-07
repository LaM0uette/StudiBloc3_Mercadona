using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using StudiBloc3_Mercadona.Model;
using StudiBloc3_Mercadona.App.Services;
using Syncfusion.Blazor.Inputs;

namespace StudiBloc3_Mercadona.App.Components.Pages;

public class HomeBase : ComponentBase
{
    #region Statements
    
    [Inject] private ApiProductService ApiProductService { get; init; } = default!;
    [Inject] private ApiCategoryService ApiCategoryService { get; init; } = default!;

    protected List<Product> Products { get; private set; } = new();
    protected readonly Product NewProduct = new();
    protected int? CurrentIdxNewProduct { get; set; } = 1;
    
    protected List<Category> Categories { get; private set; } = new();
    
    protected override async Task OnInitializedAsync()
    {
        Products = await ApiProductService.GetAllProductsAsync();
        Categories = await ApiCategoryService.GetAllCategoriesAsync();
    }

    #endregion

    #region Functions

    protected static string ImageDataUrl(byte[] imageBytes)
    {
        var base64String = Convert.ToBase64String(imageBytes);
        return $"data:image/jpeg;base64,{base64String}";
    }
    
    protected async Task HandleFileSelect(UploadChangeEventArgs args)
    {
        var file = args.Files.FirstOrDefault();
        if (file is not null)
        {
            try
            {
                await using var stream = file.File.OpenReadStream(long.MaxValue);
                var ms = new MemoryStream();
                await stream.CopyToAsync(ms);
                ms.Position = 0;
                NewProduct.Image = ms.ToArray();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
    
    protected async Task HandleSubmit()
    {
        NewProduct.CategoryId = CurrentIdxNewProduct + 1 ?? 1;
        
        await ApiProductService.AddProductAsync(NewProduct);
        Products.Add(NewProduct);
    }
    
    #endregion
}