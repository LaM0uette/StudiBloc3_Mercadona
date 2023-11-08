using StudiBloc3_Mercadona.Api.Core.Repository;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.Api.Core.Services;

public class ProductService(IRepository<Product> productRepository) : IProductService
{
    #region Tasks

    public Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return productRepository.GetAllAsync();
    }

    public Task AddProductAsync(Product product)
    {
        return productRepository.AddAsync(product);
    }
    
    public async Task<Product> AddProductTestAsync(Product product)
    {
        await productRepository.AddAsync(product);
        return product; 
    }

    #endregion
}