using StudiBloc3_Mercadona.Api.Core.Repository;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.Api.Core.Services;

public class PromotionService(IRepository<Promotion> promotionRepository) : IPromotionService
{
    public async Task<IEnumerable<Promotion>> GetAllPromotionsAsync()
    {
        var promotions = await promotionRepository.GetAllAsync();
        return promotions;
    }

    public async Task<Promotion> AddPromotionAsync(Promotion promotion)
    {
        await promotionRepository.AddAsync(promotion);
        return promotion;
    }
}