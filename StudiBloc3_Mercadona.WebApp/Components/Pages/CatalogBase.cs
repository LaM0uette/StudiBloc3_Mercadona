using Microsoft.AspNetCore.Components;
using StudiBloc3_Mercadona.Model;
using StudiBloc3_Mercadona.WebApp.Services;

namespace StudiBloc3_Mercadona.WebApp.Components.Pages;

public class CatalogBase : ComponentBase
{
    #region Statements
    
    protected IEnumerable<ProductWithPromotions> ProductsWithPromotions { get; private set; } = new List<ProductWithPromotions>();
    protected bool IsAuthenticated { get; set; } = true;
    protected Dictionary<int, string> newPromotionValues = new Dictionary<int, string>();
    
    protected class ProductWithPromotions
    {
        public Product Product { get; set; }
        public IEnumerable<Promotion> Promotions { get; set; } = new List<Promotion>();
    }

    private IEnumerable<Product> Products { get; set; } = new List<Product>();
    protected IEnumerable<Promotion> Promotions { get; set; } = new List<Promotion>();
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

            // Mise à jour du dictionnaire newPromotionValues sans réinitialiser les valeurs
            foreach (var product in Products)
            {
                if (!newPromotionValues.ContainsKey(product.Id))
                {
                    newPromotionValues[product.Id] = "0";
                }
            }
        });
    }
    
    private async Task LoadProducts() => Products = await ApiProductService.GetAllProductsAsync();
    private async Task LoadPromotions() => Promotions = await ApiPromotionService.GetAllPromotionsAsync();
    private async Task LoadProductPromotions() => ProductPromotions = await ApiProductPromotionService.GetAllProductPromotionsAsync();
    
    protected async Task AddPromotion(int productId)
    {
        if (newPromotionValues.TryGetValue(productId, out var newDiscountText) && 
            int.TryParse(newDiscountText, out var newDiscount))
        {
            var promotion = new Promotion { DiscountPercentage = newDiscount };
        
            // Recherchez une promotion existante avec le même pourcentage de réduction
            var existingPromotion = Promotions.FirstOrDefault(p => p.DiscountPercentage == promotion.DiscountPercentage);
            if (existingPromotion == null)
            {
                // Si aucune promotion existante n'est trouvée, créez-en une nouvelle
                var promotionId = await ApiPromotionService.AddPromotionAsync(promotion);
                promotion.Id = promotionId;
            }
            else
            {
                // Si une promotion existante est trouvée, utilisez son ID
                promotion.Id = existingPromotion.Id;
            }
        
            // Vérifiez si une liaison ProductPromotion existe déjà pour ce produit et cette promotion
            var existingProductPromotion = ProductPromotions.FirstOrDefault(pp => pp.ProductId == productId && pp.PromotionId == promotion.Id);
            if (existingProductPromotion == null)
            {
                // Si aucune liaison n'existe, créez-en une nouvelle
                var productPromotion = new ProductPromotion { ProductId = productId, PromotionId = promotion.Id };
                await ApiProductPromotionService.AddProductPromotionAsync(productPromotion);
            }

            // Rechargez les données pour refléter les changements
            await LoadAllDataAsync();
        }
    }
    
    protected void OnPromotionSelectionChanged(int productId, ChangeEventArgs e)
    {
        if (int.TryParse(e.Value.ToString(), out var selectedValue))
        {
            newPromotionValues[productId] = selectedValue.ToString();
        }
    }
    
    #endregion
}