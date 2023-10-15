using StudiBloc3_Mercadona.Api.Core.Repository;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.Api.Core.Services;

public class PromotionService(IRepository<Promotion> promotionRepository) : IPromotionService
{
    #region Tasks

    public Task<IEnumerable<Promotion>> GetAllPromotionsAsync()
    {
        return promotionRepository.GetAllAsync();
    }
    
    public Task<int> GetPromotionIdByDiscountPercentageAsync(int discountPercentage)
    {
        var allPromotions = promotionRepository.GetAllAsync();
        allPromotions.Wait();
        
        return Task.FromResult(allPromotions.Result.FirstOrDefault(promotion => promotion.DiscountPercentage == discountPercentage)?.Id ?? -1);
    }

    public Task AddPromotionAsync(Promotion promotion)
    {
        return promotionRepository.AddAsync(promotion);
    }

    #endregion
}