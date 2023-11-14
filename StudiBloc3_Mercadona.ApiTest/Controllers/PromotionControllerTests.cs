using Microsoft.AspNetCore.Mvc;
using Moq;
using StudiBloc3_Mercadona.Api.Controllers;
using StudiBloc3_Mercadona.Api.Core.Services;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.ApiTest.Controllers;

public class PromotionControllerTests
{
    [Fact]
    public async Task GetAll_ReturnsAllPromotions()
    {
        var mockService = new Mock<IPromotionService>();
        mockService.Setup(service => service.GetAllPromotionsAsync())
            .ReturnsAsync(new List<Promotion> { new(), new() });

        var controller = new PromotionController(mockService.Object);

        var result = await controller.GetAll();

        var promotions = Assert.IsAssignableFrom<IEnumerable<Promotion>>(result);
        Assert.Equal(2, promotions.Count());
    }
    
    [Fact]
    public async Task Add_WithAuthorizedUser_AddsPromotionAndReturnsIt()
    {
        var mockService = new Mock<IPromotionService>();
        var promotionToAdd = new Promotion();
        mockService.Setup(service => service.AddPromotionAsync(It.IsAny<Promotion>()))
            .ReturnsAsync(promotionToAdd);

        var controller = new PromotionController(mockService.Object);

        var result = await controller.Add(new Promotion());

        var actionResult = Assert.IsType<ActionResult<Promotion>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var promotionResult = okResult.Value as Promotion;
        Assert.NotNull(promotionResult);
    }
}