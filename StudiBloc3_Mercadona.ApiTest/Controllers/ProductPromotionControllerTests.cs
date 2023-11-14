using Microsoft.AspNetCore.Mvc;
using Moq;
using StudiBloc3_Mercadona.Api.Controllers;
using StudiBloc3_Mercadona.Api.Core.Services;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.ApiTest.Controllers;

public class ProductPromotionControllerTests
{
    [Fact]
    public async Task GetAll_ReturnsAllProductPromotions()
    {
        var mockService = new Mock<IProductPromotionService>();
        mockService.Setup(service => service.GetAllProductPromotionsAsync())
            .ReturnsAsync(new List<ProductPromotion> { new(), new() });

        var controller = new ProductPromotionController(mockService.Object);

        var result = await controller.GetAll();

        var productPromotions = Assert.IsAssignableFrom<IEnumerable<ProductPromotion>>(result);
        Assert.Equal(2, productPromotions.Count());
    }
    
    [Fact]
    public async Task Add_WithAuthorizedUser_AddsProductPromotionAndReturnsIt()
    {
        var mockService = new Mock<IProductPromotionService>();
        var productPromotionToAdd = new ProductPromotion();
        mockService.Setup(service => service.AddProductPromotionAsync(It.IsAny<ProductPromotion>()))
            .ReturnsAsync(productPromotionToAdd);

        var controller = new ProductPromotionController(mockService.Object);

        var result = await controller.Add(new ProductPromotion());

        var actionResult = Assert.IsType<ActionResult<ProductPromotion>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var productPromotionResult = okResult.Value as ProductPromotion;

        Assert.NotNull(productPromotionResult);
        Assert.Equal(productPromotionToAdd.EndDate, productPromotionResult.EndDate);
        Assert.Equal(productPromotionToAdd.StartDate, productPromotionResult.StartDate);
    }
    
    [Fact]
    public async Task Update_WithAuthorizedUser_UpdatesProductPromotion()
    {
        var mockService = new Mock<IProductPromotionService>();
        var controller = new ProductPromotionController(mockService.Object);

        var result = await controller.Update(new ProductPromotion());

        Assert.IsType<OkResult>(result);
    }
    
    [Fact]
    public async Task Delete_WithAuthorizedUser_DeletesProductPromotion()
    {
        var mockService = new Mock<IProductPromotionService>();
        var controller = new ProductPromotionController(mockService.Object);

        var result = await controller.Delete(new ProductPromotion());

        Assert.IsType<OkResult>(result);
    }
}