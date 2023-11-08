using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.Api.Core.Services;

public interface IProductPromotionService
{
    Task<IEnumerable<ProductPromotion>> GetAllProductPromotionsAsync();
    Task AddProductPromotionAsync(ProductPromotion productPromotion);
    Task UpdateProductPromotionAsync(ProductPromotion productPromotion);
    Task DeleteProductPromotionAsync(ProductPromotion productPromotion);
}