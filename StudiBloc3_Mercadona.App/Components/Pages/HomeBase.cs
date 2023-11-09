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
    
    // Categories
    protected List<Category> Categories { get; private set; } = new();
    private string? NewCategoryName { get; set; }

    // Promotion
    protected List<Promotion> Promotions { get; private set; } = new();
    protected readonly Product NewPromotion = new();
    private int NewPromotionsDiscount { get; set; }

    // ProductPromotion
    protected List<ProductPromotion> ProductPromotions { get; private set; } = new();
    

    protected override async Task OnInitializedAsync()
    {
        Products = await ApiProductService.GetAllProductsAsync();
        Categories = await ApiCategoryService.GetAllCategoriesAsync();
        Promotions = await ApiPromotionService.GetAllPromotionsAsync();
        ProductPromotions = await ApiProductPromotionService.GetAllProductPromotionsAsync();
    }

    #endregion

    #region Functions

    protected static string ImageDataUrl(byte[] imageBytes)
    {
        var base64String = Convert.ToBase64String(imageBytes);
        return $"data:image/jpeg;base64,{base64String}";
    }
    
    protected (float originalPrice, float discountedPrice)? CalculateDiscountedPrice(Product product)
    {
        var productPromotion = ProductPromotions.FirstOrDefault(pp => pp.ProductId == product.Id);
        if (productPromotion != null)
        {
            var promotion = Promotions.FirstOrDefault(p => p.Id == productPromotion.PromotionId);
            if (promotion != null)
            {
                var discountMultiplier = (100 - promotion.DiscountPercentage) / 100.0f;
                var discountedPrice = product.Price * discountMultiplier;
                return (product.Price, discountedPrice);
            }
        }
        return null;
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
        var productToAdd = new Product
        {
            CategoryId = NewProduct.CategoryId,
            Name = NewProduct.Name,
            Description = NewProduct.Description,
            Price = NewProduct.Price,
            Image = NewProduct.Image?.ToArray()
        };

        var addedProduct = await ApiProductService.AddProductAsync(productToAdd);
        if (addedProduct != null)
        {
            Products.Add(addedProduct); 
        }

        NewProduct.Image = null;
        CloseNewProductPopup();
        StateHasChanged();
    }
    
    protected void OpenNewProductPopup() => NewProductPopupIsVisible = true;
    
    private void CloseNewProductPopup() => NewProductPopupIsVisible = false;

    #endregion

    #region SyncFusion_Category

    protected void OnSfComboBoxCategoryChanged(string arg)
    {
        var categoryId = int.Parse(arg);
        NewProduct.CategoryId = categoryId;
    }
    
    protected Task OnSfComboBoxCategoryFiltering(FilteringEventArgs args)
    {
        NewCategoryName = args.Text;
        args.PreventDefaultAction = true;

        var query = new Query().Where(new WhereFilter
            {Field = "Name", Operator = "contains", value = args.Text, IgnoreCase = true});
        query = !string.IsNullOrEmpty(args.Text) ? query : new Query();

        return SfComboBoxNewCategory.FilterAsync(Categories, query);
    }
    
    protected async Task CreateNewCategory()
    {
        var customCategory = new Category
        {
            Name = NewCategoryName
        };
        
        var addedCategory = await ApiCategoryService.AddCategoryAsync(customCategory);
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
        NewPromotionsDiscount = Convert.ToInt32(args.Text);
        args.PreventDefaultAction = true;

        var query = new Query().Where(new WhereFilter
            {Field = "DiscountPercentage", Operator = "contains", value = args.Text, IgnoreCase = true});
        query = !string.IsNullOrEmpty(args.Text) ? query : new Query();

        return SfComboBoxNewPromotion.FilterAsync(Promotions, query);
    }
    
    protected async Task CreateNewPromotion()
    {
        var customPromotion = new Promotion
        {
            DiscountPercentage = NewPromotionsDiscount
        };
        
        var addedPromotion = await ApiPromotionService.AddPromotionAsync(customPromotion);
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
    
    private void CloseNewProductPromotionsPopup() => NewProductPromotionsPopupIsVisible = false;

    #endregion

    #region SyncFusion_ProductPromotion

    protected async Task NewProductPromotionSubmit()
    {
        var existingProductPromotion = ProductPromotions.FirstOrDefault(pp => pp.ProductId == SelectedProduct.Id);

        if (existingProductPromotion != null)
        {
            // TODO: Update product promotion
            // var index = ProductPromotions.IndexOf(existingProductPromotion);
            // var newProductPromotion = new ProductPromotion
            // {
            //     ProductId = SelectedProduct.Id,
            //     PromotionId = NewPromotionId
            // };
            //
            // ProductPromotions[index] = newProductPromotion;
            //
            // await ApiProductPromotionService.UpdateProductPromotionAsync(newProductPromotion);
        }
        else
        {
            var productPromotion = new ProductPromotion
            {
                ProductId = SelectedProduct.Id,
                PromotionId = NewPromotion.Id
            };
            
            var addedPromotion = await ApiProductPromotionService.AddProductPromotionAsync(productPromotion);
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