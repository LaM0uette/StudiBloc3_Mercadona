using StudiBloc3_Mercadona.Api.Core.Repository;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.Api.Core.Services;

public class ProductPromotionService(IRepository<ProductPromotion> productPromotionRepository) : IProductPromotionService
{
    #region Tasks

    public async Task<IEnumerable<ProductPromotion>> GetAllProductPromotionsAsync()
    {
        var productPromotions = await productPromotionRepository.GetAllAsync();
        return productPromotions;
    }

    public async Task<ProductPromotion> AddProductPromotionAsync(ProductPromotion productPromotion)
    {
        await productPromotionRepository.AddAsync(productPromotion);
        return productPromotion;
    }
    
    public Task UpdateProductPromotionAsync(ProductPromotion productPromotion)
    {
        return productPromotionRepository.UpdateAsync(productPromotion);
    }
    
    public Task DeleteProductPromotionAsync(ProductPromotion productPromotion)
    {
        return productPromotionRepository.DeleteAsync(productPromotion);
    }

    #endregion
}