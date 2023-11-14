using Microsoft.AspNetCore.Mvc;
using Moq;
using StudiBloc3_Mercadona.Api.Controllers;
using StudiBloc3_Mercadona.Api.Core.Services;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.ApiTest.Controllers;

public class CategoryControllerTests
{
    [Fact]
    public async Task GetAll_ReturnsAllCategories()
    {
        var mockService = new Mock<ICategoryService>();
        mockService.Setup(service => service.GetAllCategoriesAsync())
            .ReturnsAsync(new List<Category> { new(), new() });

        var controller = new CategoryController(mockService.Object);

        var result = await controller.GetAll();

        var categories = Assert.IsAssignableFrom<IEnumerable<Category>>(result);
        Assert.Equal(2, categories.Count());
    }
    
    [Fact]
    public async Task Add_WithAuthorizedUser_AddsCategoryAndReturnsIt()
    {
        var mockService = new Mock<ICategoryService>();
        var categoryToAdd = new Category();
        mockService.Setup(service => service.AddCategoryAsync(It.IsAny<Category>()))
            .ReturnsAsync(categoryToAdd);

        var controller = new CategoryController(mockService.Object);

        var result = await controller.Add(new Category());

        var actionResult = Assert.IsType<ActionResult<Category>>(result);
        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var categoryResult = okResult.Value as Category;
        Assert.NotNull(categoryResult);
        Assert.Equal(categoryToAdd.Id, categoryResult.Id);
        Assert.Equal(categoryToAdd.Name, categoryResult.Name);
    }
}