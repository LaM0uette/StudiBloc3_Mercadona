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
    [Inject] private ApiPromotionService ApiPromotionService { get; init; } = default!;
    [Inject] private ApiProductPromotionService ApiProductPromotionService { get; init; } = default!;
    
    // SyncFusion
    protected SfComboBox<string, Category> SfComboBoxNewCategory = null!;
    protected SfComboBox<int, Promotion> SfComboBoxNewPromotion = null!;
    protected bool NewProductPopupIsVisible { get; set; }
    protected bool NewProductPromotionsPopupIsVisible { get; set; }

    // Products
    protected List<Product> Products { get; private set; } = new();
    protected readonly Product NewProduct = new();
    private Product SelectedProduct = new();
    
    // Products Filter
    private int? SelectedCategoryId { get; set; }
    protected IEnumerable<Product> FilteredProducts =>
        SelectedCategoryId.HasValue
            ? Products.Where(p => p.CategoryId == SelectedCategoryId.Value)
            : Products;
    
    // Categories
    protected List<Category> Categories { get; private set; } = new();
    private string? NewCategoryName { get; set; }

    // Promotion TODO: ajouter les date de début et de fin de la promotion !
    protected List<Promotion> Promotions { get; private set; } = new();
    private readonly Product NewPromotion = new();

    // ProductPromotion
    protected List<ProductPromotion> ProductPromotions { get; set; } = new();
    protected ProductPromotion NewProductPromotion { get; set; } = new()
    {
        StartDate = DateTime.Today,
        EndDate = DateTime.Today.AddDays(7)
    };
    private int CustomPromotionsDiscount { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Products = await ApiProductService.GetAllProductsAsync();
        Categories = await ApiCategoryService.GetAllCategoriesAsync();
        Promotions = await ApiPromotionService.GetAllPromotionsAsync();
        ProductPromotions = await ApiProductPromotionService.GetAllProductPromotionsAsync();
        
        await DeleteExpiredPromotions();
    }

    #endregion

    #region Functions

    protected static string ImageBytesToImageDataUrl(byte[] imageBytes)
    {
        var base64String = Convert.ToBase64String(imageBytes);
        return $"data:image/jpeg;base64,{base64String}";
    }
    
    protected (float originalPrice, float discountedPrice)? CalculateDiscountedPrice(Product product)
    {
        var productPromotion = ProductPromotions.FirstOrDefault(pp => pp.ProductId == product.Id 
                                                                      && pp.StartDate <= DateTime.UtcNow 
                                                                      && pp.EndDate.AddDays(1) > DateTime.UtcNow);
        if (productPromotion is null) return null;
    
        var promotion = Promotions.FirstOrDefault(p => p.Id == productPromotion.PromotionId);
        if (promotion is null) return null;
    
        var discountMultiplier = (100 - promotion.DiscountPercentage) / 100f;
        var discountedPrice = product.Price * discountMultiplier;
    
        return (product.Price, discountedPrice);
    }
    
    protected int GetRemainingDaysForPromotion(ProductPromotion? promotion)
    {
        if (promotion is null)
        {
            return 0;
        }

        var endDate = promotion.EndDate.Date.AddDays(1);
        var today = DateTime.UtcNow.Date;
        return (endDate - today).Days;
    }

    
    protected string GetProductCategoryName(Product product)
    {
        var category = Categories.FirstOrDefault(c => c.Id == product.CategoryId);
        return category?.Name ?? "Unknown";
    }

    private async Task DeleteExpiredPromotions()
    {
        //This function should be launched at regular intervals from the backend but for simplicity I added it here.
        
        var expiredPromotions = ProductPromotions.Where(pp => pp.EndDate < DateTime.Today).ToList();

        foreach (var expiredPromotion in expiredPromotions)
        {
            try
            {
                await ApiProductPromotionService.DeleteProductPromotionAsync(expiredPromotion);
                ProductPromotions.Remove(expiredPromotion);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        StateHasChanged();
    }
    
    #endregion

    #region SyncFusion
    
    #region SyncFusion_NewProduct

    protected async Task OnSfUploaderNewProductImageChanged(UploadChangeEventArgs args)
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
    
    protected async Task NewProductSubmit()
    {
        var newProduct = new Product
        {
            CategoryId = NewProduct.CategoryId,
            Name = NewProduct.Name,
            Description = NewProduct.Description,
            Price = NewProduct.Price,
            Image = NewProduct.Image?.ToArray()
        };

        var addedProduct = await ApiProductService.AddProductAsync(newProduct);
        if (addedProduct != null)
        {
            Products.Add(addedProduct); 
        }

        NewProduct.Image = null;
        CloseNewProductPopup();
        StateHasChanged();
    }

    protected void OpenNewProductPopup()
    {
        NewProductPopupIsVisible = true;
    }

    private void CloseNewProductPopup()
    {
        NewProductPopupIsVisible = false;
    }

    #endregion

    #region SyncFusion_Category
    
    protected void OnSfComboBoxSelectCategoryChanged(string arg)
    {
        if (arg is "" or null)
        {
            SelectedCategoryId = null;
            return;
        }
        
        var categoryId = int.Parse(arg);
        SelectedCategoryId = categoryId;
    }

    protected void OnSfComboBoxCategoryChanged(string arg)
    {
        var categoryId = int.Parse(arg);
        NewProduct.CategoryId = categoryId;
    }
    
    protected Task OnSfComboBoxCategoryFiltering(FilteringEventArgs args)
    {
        NewCategoryName = args.Text;
        args.PreventDefaultAction = true;

        var query = new Query().Where(new WhereFilter{Field = "Name", Operator = "contains", value = args.Text, IgnoreCase = true});
        query = !string.IsNullOrEmpty(args.Text) ? query : new Query();

        return SfComboBoxNewCategory.FilterAsync(Categories, query);
    }
    
    protected async Task CreateNewCategory()
    {
        var newCategory = new Category
        {
            Name = NewCategoryName
        };
        
        var addedCategory = await ApiCategoryService.AddCategoryAsync(newCategory);
        if (addedCategory != null)
        {
            await SfComboBoxNewCategory.AddItemsAsync(new List<Category> {addedCategory});
            Categories.Add(addedCategory);
        }
        
        await SfComboBoxNewCategory.HidePopupAsync();
        StateHasChanged();
    }
    
    #endregion

    #region SyncFusion_Promotion
    
    protected void OnSfComboBoxPromotionChanged(int arg)
    {
        NewPromotion.Id = arg;
    }
    
    protected Task OnSfComboBoxPromotionFiltering(FilteringEventArgs args)
    {
        CustomPromotionsDiscount = Convert.ToInt32(args.Text);
        args.PreventDefaultAction = true;

        var query = new Query().Where(new WhereFilter{Field = "DiscountPercentage", Operator = "contains", value = args.Text, IgnoreCase = true});
        query = !string.IsNullOrEmpty(args.Text) ? query : new Query();

        return SfComboBoxNewPromotion.FilterAsync(Promotions, query);
    }
    
    protected async Task CreateNewPromotion()
    {
        var newPromotion = new Promotion
        {
            DiscountPercentage = CustomPromotionsDiscount
        };
        
        var addedPromotion = await ApiPromotionService.AddPromotionAsync(newPromotion);
        if (addedPromotion != null)
        {
            await SfComboBoxNewPromotion.AddItemsAsync(new List<Promotion> {addedPromotion});
            Promotions.Add(addedPromotion);
        }

        await SfComboBoxNewPromotion.HidePopupAsync();
        StateHasChanged();
    }

    protected void OpenNewProductPromotionsPopup(Product product)
    {
        SelectedProduct = product;
        NewProductPromotionsPopupIsVisible = true;
    }

    private void CloseNewProductPromotionsPopup()
    {
        NewProductPromotionsPopupIsVisible = false;
    }

    #endregion

    #region SyncFusion_ProductPromotion

    protected async Task NewProductPromotionSubmit()
    {
        var existingProductPromotion = ProductPromotions.FirstOrDefault(pp => pp.ProductId == SelectedProduct.Id);
        var utcStartDate = DateTime.SpecifyKind(NewProductPromotion.StartDate, DateTimeKind.Utc);
        var utcEndDate = DateTime.SpecifyKind(NewProductPromotion.EndDate, DateTimeKind.Utc);
        
        if (existingProductPromotion is not null)
        {
            var newProductPromotion = new ProductPromotion
            {
                Id = existingProductPromotion.Id,
                ProductId = existingProductPromotion.ProductId,
                StartDate = utcStartDate,
                EndDate = utcEndDate,
                PromotionId = NewPromotion.Id
            };
            
            var index = ProductPromotions.IndexOf(existingProductPromotion);
            ProductPromotions[index] = newProductPromotion;
            
            await ApiProductPromotionService.UpdateProductPromotionAsync(newProductPromotion);
        }
        else
        {
            var newProductPromotion = new ProductPromotion
            {
                ProductId = SelectedProduct.Id,
                PromotionId = NewPromotion.Id,
                StartDate = utcStartDate,
                EndDate = utcEndDate
            };
            
            var addedPromotion = await ApiProductPromotionService.AddProductPromotionAsync(newProductPromotion);
            if (addedPromotion != null)
            {
                ProductPromotions.Add(addedPromotion);
            }
        }
        
        CloseNewProductPromotionsPopup();
    }

    #endregion
    
    #endregion
}