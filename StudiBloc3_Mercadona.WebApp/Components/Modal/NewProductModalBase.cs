using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using StudiBloc3_Mercadona.Model;
using StudiBloc3_Mercadona.WebApp.Services;

namespace StudiBloc3_Mercadona.WebApp.Components.Modal;

public class NewProductModalBase : ComponentBase
{
    #region Statements

    [Parameter] public bool IsOpen { get; set; }
    [Parameter] public EventCallback OnClose { get; set; }
    [Parameter] public EventCallback<Product> OnSubmit { get; set; }
    [Parameter] public EventCallback<ChangeEventArgs> OnChange { get; set; }
    
    protected IEnumerable<Category> Categories { get; set; } = new List<Category>();
    [Inject] private ApiCategoryService ApiCategoryService { get; set; } = default!;
    
    protected readonly Dictionary<int, string> CategoriesValues = new();
    protected Product Product { get; set; } = new();
    private byte[]? imageBytes;

    #endregion

    #region Functions
    
    protected override async Task OnInitializedAsync()
    {
        await LoadCategories();
        if (Categories.Any())
        {
            Product.CategoryId = Categories.First().Id;
        }
    }
    protected string GetCategoryValue(int categoryId)
    {
        if (CategoriesValues.TryGetValue(categoryId, out var value))
        {
            return value;
        }
        return "0";  // ou toute autre valeur par défaut
    }
    protected void SetCategoryValue(int categoryId, string newValue)
    {
        CategoriesValues[categoryId] = newValue;
        Product.CategoryId = categoryId;
    }

    protected void HandleCategoryChanged(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out var categoryId))
        {
            Product.CategoryId = categoryId;
        }
    }
    
    protected async Task HandleFileSelected(InputFileChangeEventArgs e)
    {
        var file = e.File;
        if (file != null)
        {
            using var stream = file.OpenReadStream(maxAllowedSize: 1952975);  // Limite de taille à 1.9MB, ajustez en conséquence
            var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            imageBytes = ms.ToArray();
        }
    }

    protected async Task HandleSubmit()
    {
        if (imageBytes != null)
        {
            Console.WriteLine("ImageBytes is not null");
            Product.Image = imageBytes;
        }
            
        await OnSubmit.InvokeAsync(Product);
    }
    
    protected Task HandleClose() => OnClose.InvokeAsync(null);

    private async Task LoadCategories()
    {
        Categories = await ApiCategoryService.GetAllCategoriesAsync();
        
        foreach (var category in Categories)
            CategoriesValues.TryAdd(category.Id, "0");
    }

    #endregion
}