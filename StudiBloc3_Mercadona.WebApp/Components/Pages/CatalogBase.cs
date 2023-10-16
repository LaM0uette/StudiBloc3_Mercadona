using Microsoft.AspNetCore.Components;
using StudiBloc3_Mercadona.Model;
using StudiBloc3_Mercadona.WebApp.Services;

namespace StudiBloc3_Mercadona.WebApp.Components.Pages;

public class CatalogBase : ComponentBase
{
    #region Statements

    protected bool IsAuthenticated { get; set; } = true;

    protected IEnumerable<Product> Products { get; set; } = new List<Product>();
    protected IEnumerable<Promotion> Promotions { get; set; } = new List<Promotion>();
    private IEnumerable<ProductPromotion> ProductPromotions { get; set; } = new List<ProductPromotion>();
    private IEnumerable<Category> Categories { get; set; } = new List<Category>();
    protected IEnumerable<FullProduct> FullProducts { get; private set; } = new List<FullProduct>();

    [Inject] private ApiProductService ApiProductService { get; set; } = default!;
    [Inject] private ApiPromotionService ApiPromotionService { get; set; } = default!;
    [Inject] private ApiProductPromotionService ApiProductPromotionService { get; set; } = default!;
    [Inject] private ApiCategoryService ApiCategoryService { get; set; } = default!;

    protected readonly Dictionary<int, string> newPromotionValues = new();
    protected bool isModalOpen;

    protected override Task OnInitializedAsync()
    {
        return LoadAllDataAsync();
    }

    protected string DisplayProductName(Product product)
    {
        return product?.Name ?? "Unnamed Product";
    }
    
    #endregion

    #region LoadData

    private Task LoadAllDataAsync()
    {
        var loadProductsTask = LoadProducts();
        var loadPromotionsTask = LoadPromotions();
        var loadProductPromotionsTask = LoadProductPromotions();
        var loadCategoriesTask = LoadCategories();

        return Task.WhenAll(loadProductsTask, loadPromotionsTask, loadProductPromotionsTask, loadCategoriesTask).ContinueWith(_ =>
        {
            var fullProducts = from product in Products
                join productPromotion in ProductPromotions
                    on product.Id equals productPromotion.ProductId into productPromotionGroup
                from productPromotion in productPromotionGroup.DefaultIfEmpty()
                join promotion in Promotions
                    on productPromotion?.PromotionId equals promotion.Id into promotionGroup
                from promotion in promotionGroup.DefaultIfEmpty()
                join category in Categories
                    on product.CategoryId equals category.Id into categoryGroup
                from category in categoryGroup.DefaultIfEmpty()
                select new FullProduct
                {
                    Product = product,
                    Promotion = promotion,
                    ProductPromotion = productPromotion,
                    Category = category
                };
            
            FullProducts = fullProducts.ToList();
            
            foreach (var product in Products)
                newPromotionValues.TryAdd(product.Id, "0");
        });
    }
    
    private async Task LoadProducts() => Products = await ApiProductService.GetAllProductsAsync();
    private async Task LoadPromotions() => Promotions = await ApiPromotionService.GetAllPromotionsAsync();
    private async Task LoadProductPromotions() => ProductPromotions = await ApiProductPromotionService.GetAllProductPromotionsAsync();
    private async Task LoadCategories() => Categories = await ApiCategoryService.GetAllCategoriesAsync();
    #endregion

    #region Functions
    
    protected async Task AddProduct(Product newProduct)
    {
        await ApiProductService.AddProductAsync(newProduct);
        await LoadAllDataAsync();
        
        CloseModal();
    }
    
    protected async Task AddPromotion(int productId)
    {
        if (newPromotionValues.TryGetValue(productId, out var promotionText) && int.TryParse(promotionText, out var promotionValue))
        {
            var promotion = new Promotion {DiscountPercentage = promotionValue};

            var existingPromotion = Promotions.FirstOrDefault(p => p.DiscountPercentage == promotion.DiscountPercentage);
            if (existingPromotion is null)
            {
                promotion.Id = await ApiPromotionService.AddPromotionAsync(promotion);
            }
            else
            {
                promotion.Id = existingPromotion.Id;
            }

            var existingProductPromotion = ProductPromotions.FirstOrDefault(pp => pp.ProductId == productId && pp.PromotionId == promotion.Id);
            if (existingProductPromotion is null)
            {
                var productPromotion = new ProductPromotion {ProductId = productId, PromotionId = promotion.Id};
                await ApiProductPromotionService.AddProductPromotionAsync(productPromotion);
            }

            await LoadAllDataAsync();
        }
    }
    
    public string ImageDataUrl(byte[] imageBytes)
    {
        var base64String = Convert.ToBase64String(imageBytes);
        return $"data:image/jpeg;base64,{base64String}";
    }

    #endregion

    #region Modal

    protected void OpenModal() => isModalOpen = true;
    protected void CloseModal() => isModalOpen = false;

    #endregion
}