using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.Core.Services;

public interface IProductService
{
    Task<IEnumerable<Product>> GetAllProductsAsync();
    Task AddProductAsync(Product product);
}