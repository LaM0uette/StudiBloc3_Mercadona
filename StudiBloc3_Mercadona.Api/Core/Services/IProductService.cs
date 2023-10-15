using StudiBloc3_Mercadona.Api.Model;

namespace StudiBloc3_Mercadona.Api.Core.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task AddProductAsync(Product product);
}