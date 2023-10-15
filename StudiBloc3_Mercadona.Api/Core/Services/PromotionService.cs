using StudiBloc3_Mercadona.Api.Core.Repository;
using StudiBloc3_Mercadona.Api.Model;

namespace StudiBloc3_Mercadona.Api.Core.Services;

public class PromotionService(IRepository<Promotion> promotionRepository) : IPromotionService
{
    #region Tasks

    public Task<IEnumerable<Promotion>> GetAllPromotionsAsync()
    {
        return promotionRepository.GetAllAsync();
    }

    public Task AddPromotionAsync(Promotion promotion)
    {
        return promotionRepository.AddAsync(promotion);
    }

    #endregion
}