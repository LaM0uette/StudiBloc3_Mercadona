using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.Api.Core.Services;

public interface IPromotionService
{
    Task<IEnumerable<Promotion>> GetAllPromotionsAsync();
    Task<int> GetPromotionIdByDiscountPercentageAsync(int discountPercentage);
    Task AddPromotionAsync(Promotion promotion);
}