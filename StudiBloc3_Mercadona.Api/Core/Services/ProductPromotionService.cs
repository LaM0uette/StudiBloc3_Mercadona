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
    
    public async Task UpdateProductPromotionAsync(ProductPromotion productPromotion)
    {
        await productPromotionRepository.UpdateAsync(productPromotion);
    }
    
    public async Task DeleteProductPromotionAsync(ProductPromotion productPromotion)
    {
        await productPromotionRepository.DeleteAsync(productPromotion);
    }

    #endregion
}