using StudiBloc3_Mercadona.Api.Model;

namespace StudiBloc3_Mercadona.Api.Core.Services;

public interface IPromotionService
{
    Task<IEnumerable<Promotion>> GetAllPromotionsAsync();
    Task AddPromotionAsync(Promotion promotion);
}