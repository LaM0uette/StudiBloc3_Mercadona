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

    protected async Task HandleFileSelect(InputFileChangeEventArgs e)
    {
        var imageFiles = e.GetMultipleFiles();
        var imageFile = imageFiles.FirstOrDefault();
        if (imageFile != null)
        {
            await using var stream = imageFile.OpenReadStream(maxAllowedSize: 1024 * 1024);
            var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            NewProduct.Image = ms.ToArray();
        }
    }
    protected async Task OnChange(UploadChangeEventArgs args)
    {
        try
        {
            foreach (var file in args.Files)
            {
                var path = @"" + file.FileInfo.Name;
                var filestream = new FileStream(path, FileMode.Create, FileAccess.Write);
                
                await using var stream = file.File.OpenReadStream(long.MaxValue);
                var ms = new MemoryStream();
                await stream.CopyToAsync(ms);
                NewProduct.Image = ms.ToArray();
                
                filestream.Close();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
    
    protected async Task HandleSubmit()
    {
        await ApiProductService.AddProductAsync(NewProduct);
        
        Products.Add(NewProduct);
    }

    protected void TEST()
    {
        NewProduct.CategoryId = CurrentIdxNewProduct ?? 1;
        
        Console.WriteLine(NewProduct.CategoryId);
        Console.WriteLine(NewProduct.Name);
        Console.WriteLine(NewProduct.Description);
        Console.WriteLine(NewProduct.Price);
        Console.WriteLine(NewProduct.Image);
    }
    
    #endregion
}