using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.Core.Services;

public interface IPromotionService
{
    Task<IEnumerable<Promotion>> GetAllPromotionsAsync();
    Task AddPromotionAsync(Promotion promotion);
}