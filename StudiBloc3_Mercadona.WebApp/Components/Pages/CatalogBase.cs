using Microsoft.AspNetCore.Components;
using StudiBloc3_Mercadona.Model;
using StudiBloc3_Mercadona.WebApp.Services;

namespace StudiBloc3_Mercadona.WebApp.Components.Pages;

public class CatalogBase : ComponentBase
{
    #region Statements
    
    protected IEnumerable<ProductWithPromotions> ProductsWithPromotions { get; private set; } = new List<ProductWithPromotions>();

    protected class ProductWithPromotions
    {
        public Product Product { get; set; }
        public IEnumerable<Promotion> Promotions { get; set; } = new List<Promotion>();
    }

    private IEnumerable<Product> Products { get; set; } = new List<Product>();
    private IEnumerable<Promotion> Promotions { get; set; } = new List<Promotion>();
    private IEnumerable<ProductPromotion> ProductPromotions { get; set; } = new List<ProductPromotion>();
    
    [Inject] private ApiProductService ApiProductService { get; set; } = default!;
    [Inject] private ApiPromotionService ApiPromotionService { get; set; } = default!;
    [Inject] private ApiProductPromotionService ApiProductPromotionService { get; set; } = default!;

    #endregion

    #region Functions

    protected override Task OnInitializedAsync()
    {
        return LoadAllDataAsync();
    }
    
    private Task LoadAllDataAsync()
    {
        var loadProductsTask = LoadProducts();
        var loadPromotionsTask = LoadPromotions();
        var loadProductPromotionsTask = LoadProductPromotions();

        //return Task.WhenAll(loadProductsTask, loadPromotionsTask, loadProductPromotionsTask);
        return Task.WhenAll(loadProductsTask, loadPromotionsTask, loadProductPromotionsTask).ContinueWith(_ =>
        {
            var productPromotionGroups =
                from p in Products
                join pp in ProductPromotions on p.Id equals pp.ProductId into ppg
                from pp in ppg.DefaultIfEmpty()
                join promo in Promotions on pp?.PromotionId equals promo.Id into promoGroup
                from promo in promoGroup.DefaultIfEmpty()
                group promo by p into g
                select new ProductWithPromotions
                {
                    Product = g.Key,
                    Promotions = g.Where(promo => promo != null)
                };

            ProductsWithPromotions = productPromotionGroups.ToList();
        });
    }
    
    private async Task LoadProducts() => Products = await ApiProductService.GetAllProductsAsync();
    private async Task LoadPromotions() => Promotions = await ApiPromotionService.GetAllPromotionsAsync();
    private async Task LoadProductPromotions() => ProductPromotions = await ApiProductPromotionService.GetAllProductPromotionsAsync();

    protected async Task AddPromotion(int productId)
    {
        var promotion = new Promotion { DiscountPercentage = 40 };
        
        var existingPromotion = Promotions.FirstOrDefault(p => p.DiscountPercentage == promotion.DiscountPercentage);
        if (existingPromotion == null)
        {
            await ApiPromotionService.AddPromotionAsync(promotion);
            await LoadAllDataAsync();
        }
        
        var existingProductPromotion = ProductPromotions.FirstOrDefault(pp => pp.ProductId == productId);
        if (existingProductPromotion == null)
        {
            var promo = Promotions.FirstOrDefault(p => p.DiscountPercentage == promotion.DiscountPercentage);
            var productPromotion = new ProductPromotion { ProductId = productId, PromotionId = promo.Id };
            await ApiProductPromotionService.AddProductPromotionAsync(productPromotion);
            await LoadAllDataAsync();
        }
    }
    
    #endregion
}