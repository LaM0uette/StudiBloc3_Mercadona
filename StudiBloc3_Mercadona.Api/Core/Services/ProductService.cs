using StudiBloc3_Mercadona.Api.Core.Repository;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.Api.Core.Services;

public class ProductService(IRepository<Product> productRepository) : IProductService
{
    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        var products = await productRepository.GetAllAsync();
        return products;
    }
    
    public async Task<Product> AddProductAsync(Product product)
    {
        await productRepository.AddAsync(product);
        return product; 
    }
}