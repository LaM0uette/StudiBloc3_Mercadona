using StudiBloc3_Mercadona.Api.Core.Repository;
using StudiBloc3_Mercadona.Api.Model;

namespace StudiBloc3_Mercadona.Api.Core.Services;

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