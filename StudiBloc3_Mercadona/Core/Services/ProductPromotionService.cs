using StudiBloc3_Mercadona.Core.Repository;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.Core.Services;

public class ProductPromotionService(IRepository<ProductPromotion> productPromotionRepository) : IProductPromotionService
{
    #region Tasks

    public Task<IEnumerable<ProductPromotion>> GetAllProductPromotionsAsync()
    {
        return productPromotionRepository.GetAllAsync();
    }

    public Task AddProductPromotionAsync(ProductPromotion productPromotion)
    {
        return productPromotionRepository.AddAsync(productPromotion);
    }

    #endregion
}