using StudiBloc3_Mercadona.Api.Core.Repository;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.Api.Core.Services;

public class PromotionService(IRepository<Promotion> promotionRepository) : IPromotionService
{
    #region Tasks

    public async Task<IEnumerable<Promotion>> GetAllPromotionsAsync()
    {
        var promotions = await promotionRepository.GetAllAsync();
        return promotions;
    }
    
    public Task<int> GetPromotionIdByDiscountPercentageAsync(int discountPercentage)
    {
        var allPromotions = promotionRepository.GetAllAsync();
        allPromotions.Wait();
        
        return Task.FromResult(allPromotions.Result.FirstOrDefault(promotion => promotion.DiscountPercentage == discountPercentage)?.Id ?? -1);
    }

    public async Task<Promotion> AddPromotionAsync(Promotion promotion)
    {
        await promotionRepository.AddAsync(promotion);
        return promotion;
    }

    #endregion
}