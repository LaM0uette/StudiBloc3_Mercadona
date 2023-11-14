using Microsoft.AspNetCore.Mvc;
using Moq;
using StudiBloc3_Mercadona.Api.Controllers;
using StudiBloc3_Mercadona.Api.Core.Services;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.ApiTest.Controllers;

public class ProductControllerTests
{
    [Fact]
    public async Task GetAll_ReturnsAllProducts()
    {
        var mockService = new Mock<IProductService>();
        mockService.Setup(service => service.GetAllProductsAsync())
            .ReturnsAsync(new List<Product> { new(), new() });

        var controller = new ProductController(mockService.Object);

        var result = await controller.GetAll();

        var products = Assert.IsAssignableFrom<IEnumerable<Product>>(result);
        Assert.Equal(2, products.Count());
    }
    
    [Fact]
    public async Task Add_WithAuthorizedUser_AddsProductAndReturnsIt()
    {
        var mockService = new Mock<IProductService>();
        var productToAdd = new Product();
        mockService.Setup(service => service.AddProductAsync(It.IsAny<Product>()))
            .ReturnsAsync(productToAdd);

        var controller = new ProductController(mockService.Object);

        var result = await controller.Add(new Product());

        var actionResult = Assert.IsType<ActionResult<Product>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var productResult = okResult.Value as Product;

        Assert.NotNull(productResult);
        Assert.Equal(productToAdd.CategoryId, productResult.CategoryId);
        Assert.Equal(productToAdd.Description, productResult.Description);
    }
}