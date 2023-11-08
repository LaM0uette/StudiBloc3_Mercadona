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

    // Products
    protected List<Product> Products { get; private set; } = new();
    protected readonly Product NewProduct = new();
    protected Product SelectedProduct = new();
    protected bool NewProductPopupIsVisible { get; set; }

    // Categories
    protected List<Category> Categories { get; private set; } = new();
    protected SfComboBox<string, Category> SfComboBoxNewCategory = null!;
    private string? NewCategoryName { get; set; }

    // Promotion
    protected List<Promotion> Promotions { get; private set; } = new();
    protected readonly Product NewPromotion = new();
    protected SfComboBox<int, Promotion> SfComboBoxNewPromotions = null!;
    private int NewPromotionsDiscount { get; set; }

    // ProductPromotion
    protected List<ProductPromotion> ProductPromotions { get; private set; } = new();
    protected bool NewProductPromotionsPopupIsVisible { get; set; }

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
            Id = Products.Max(c => c.Id) + 1,
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

    protected async Task AddPromotionToProduct()
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

            await ApiProductPromotionService.AddProductPromotionAsync(productPromotion);
            ProductPromotions.Add(productPromotion);
        }
        
        CloseNewProductPromotionsPopup();
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

    // Category
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
            Id = Categories.Max(c => c.Id) + 1,
            Name = NewCategoryName
        };

        await ApiCategoryService.AddCategoryAsync(customCategory);
        await SfComboBoxNewCategory.AddItemsAsync(new List<Category> {customCategory});
        Categories.Add(customCategory);

        await SfComboBoxNewCategory.HidePopupAsync();
    }

    protected void OpenNewProductPopup() => NewProductPopupIsVisible = true;
    private void CloseNewProductPopup() => NewProductPopupIsVisible = false;

    // Promotion
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

        return SfComboBoxNewPromotions.FilterAsync(Promotions, query);
    }

    protected async Task CreateNewPromotion()
    {
        var customPromotion = new Promotion
        {
            Id = Promotions.Max(c => c.Id) + 1,
            DiscountPercentage = NewPromotionsDiscount
        };

        await ApiPromotionService.AddPromotionAsync(customPromotion);
        await SfComboBoxNewPromotions.AddItemsAsync(new List<Promotion> {customPromotion});
        Promotions.Add(customPromotion);

        await SfComboBoxNewPromotions.HidePopupAsync();
    }
    
    protected void OpenNewProductPromotionsPopup(Product product)
    {
        // Stockez le produit sélectionné dans une propriété afin de pouvoir l'utiliser plus tard pour ajouter la promotion
        SelectedProduct = product;
        NewProductPromotionsPopupIsVisible = true;
    }
    private void CloseNewProductPromotionsPopup() => NewProductPromotionsPopupIsVisible = false;

    #endregion
}