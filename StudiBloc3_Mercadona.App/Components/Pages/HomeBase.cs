using Microsoft.AspNetCore.Components;
using StudiBloc3_Mercadona.Model;
using StudiBloc3_Mercadona.App.Services;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Inputs;

namespace StudiBloc3_Mercadona.App.Components.Pages;

public class HomeBase : ComponentBase
{
    #region Statements
    
    // Services
    [Inject] private ApiProductService ApiProductService { get; init; } = default!;
    [Inject] private ApiCategoryService ApiCategoryService { get; init; } = default!;

    // Products
    protected List<Product> Products { get; private set; } = new();
    protected readonly Product NewProduct = new();
    protected bool NewProductPopupIsVisible { get; set; }
    
    // Categories
    protected List<Category> Categories { get; private set; } = new();
    protected SfComboBox<string, Category> SfComboBoxNewCategory = null!;
    private string? NewCategoryName { get; set; }
    
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
                NewProduct.Image = null;
            }
        }
    }
    
    protected async Task HandleSubmit()
    {
        var productToAdd = new Product
        {
            CategoryId = NewProduct.CategoryId,
            Name = NewProduct.Name,
            Description = NewProduct.Description,
            Price = NewProduct.Price,
            Image = NewProduct.Image?.ToArray()
        };

        await ApiProductService.AddProductAsync(productToAdd);
        Products.Add(productToAdd);

        NewProduct.Image = null;
        CloseNewProductPopup();
    }
    
    #endregion

    #region SyncFusion
    
    protected void OnSfComboBoxCategoryChanged(string arg)
    {
        Console.WriteLine(arg);
        var categoryId = int.Parse(arg);
        NewProduct.CategoryId = categoryId;
    }
    
    protected Task OnSfComboBoxCategoryFiltering(FilteringEventArgs args)
    {
        NewCategoryName = args.Text;
        args.PreventDefaultAction = true;
        
        var query = new Query().Where(new WhereFilter{ Field = "Name", Operator = "contains", value = args.Text, IgnoreCase = true });
        query = !string.IsNullOrEmpty(args.Text) ? query : new Query();
        
        return SfComboBoxNewCategory.FilterAsync(Categories, query);
    }
    
    protected async Task CreateNewCategory()
    {
        var customCategory = new Category
        {
            Id = Categories.Max(c => c.Id) + 1,
            Name = NewCategoryName 
        };
        
        await ApiCategoryService.AddCategoryAsync(customCategory);
        await SfComboBoxNewCategory.AddItemsAsync(new List<Category> { customCategory });
        Categories.Add(customCategory);
        
        await SfComboBoxNewCategory.HidePopupAsync();
    }

    protected void OpenNewProductPopup() => NewProductPopupIsVisible = true;
    private void CloseNewProductPopup() => NewProductPopupIsVisible = false;

    #endregion
    
    
    
    
}