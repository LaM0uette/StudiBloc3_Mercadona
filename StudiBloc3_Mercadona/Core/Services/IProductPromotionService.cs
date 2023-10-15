using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.Core.Services;

public interface IProductPromotionService
{
    Task<IEnumerable<ProductPromotion>> GetAllProductPromotionsAsync();
    Task AddProductPromotionAsync(ProductPromotion productPromotion);
}