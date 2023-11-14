using Moq;
using StudiBloc3_Mercadona.Api.Core.Repository;
using StudiBloc3_Mercadona.Api.Core.Services;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.ApiTest.Services;

public class CategoryServiceTests
{
    [Fact]
    public async Task GetAllCategoriesAsync_ReturnsAllCategories()
    {
        var mockRepo = new Mock<IRepository<Category>>();
        mockRepo.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(new List<Category> { new(), new() });

        var service = new CategoryService(mockRepo.Object);

        var result = await service.GetAllCategoriesAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }
    
    [Fact]
    public async Task AddCategoryAsync_AddsCategoryAndReturnsIt()
    {
        var categoryToAdd = new Category();
        var mockRepo = new Mock<IRepository<Category>>();
        mockRepo.Setup(repo => repo.AddAsync(It.IsAny<Category>()))
            .Callback<Category>(c => categoryToAdd = c);

        var service = new CategoryService(mockRepo.Object);

        var result = await service.AddCategoryAsync(new Category());

        Assert.NotNull(result);
        Assert.Equal(categoryToAdd, result);
    }
}