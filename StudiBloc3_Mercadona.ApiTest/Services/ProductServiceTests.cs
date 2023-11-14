using Moq;
using StudiBloc3_Mercadona.Api.Core.Repository;
using StudiBloc3_Mercadona.Api.Core.Services;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.ApiTest.Services;

public class ProductServiceTests
{
    [Fact]
    public async Task GetAllProductsAsync_ReturnsAllProducts()
    {
        var mockRepo = new Mock<IRepository<Product>>();
        mockRepo.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(new List<Product> { new(), new() });

        var service = new ProductService(mockRepo.Object);

        var result = await service.GetAllProductsAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }
    
    [Fact]
    public async Task AddProductAsync_AddsProductAndReturnsIt()
    {
        var productToAdd = new Product();
        var mockRepo = new Mock<IRepository<Product>>();
        mockRepo.Setup(repo => repo.AddAsync(It.IsAny<Product>()))
            .Callback<Product>(p => productToAdd = p);

        var service = new ProductService(mockRepo.Object);

        var result = await service.AddProductAsync(new Product());

        Assert.NotNull(result);
        Assert.Equal(productToAdd, result);
    }
}