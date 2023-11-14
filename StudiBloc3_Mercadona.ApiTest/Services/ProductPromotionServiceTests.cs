using Moq;
using StudiBloc3_Mercadona.Api.Core.Repository;
using StudiBloc3_Mercadona.Api.Core.Services;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.ApiTest.Services;

public class ProductPromotionServiceTests
{
    [Fact]
    public async Task GetAllProductPromotionsAsync_ReturnsAllProductPromotions()
    {
        var mockRepo = new Mock<IRepository<ProductPromotion>>();
        mockRepo.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(new List<ProductPromotion> { new(), new() });

        var service = new ProductPromotionService(mockRepo.Object);

        var result = await service.GetAllProductPromotionsAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }
    
    [Fact]
    public async Task AddProductPromotionAsync_AddsProductPromotionAndReturnsIt()
    {
        var productPromotionToAdd = new ProductPromotion();
        var mockRepo = new Mock<IRepository<ProductPromotion>>();
        mockRepo.Setup(repo => repo.AddAsync(It.IsAny<ProductPromotion>()))
            .Callback<ProductPromotion>(pp => productPromotionToAdd = pp);

        var service = new ProductPromotionService(mockRepo.Object);

        var result = await service.AddProductPromotionAsync(new ProductPromotion());

        Assert.NotNull(result);
        Assert.Equal(productPromotionToAdd, result);
    }
    
    [Fact]
    public async Task UpdateProductPromotionAsync_UpdatesProductPromotion()
    {
        var mockRepo = new Mock<IRepository<ProductPromotion>>();
        var productPromotionToUpdate = new ProductPromotion();

        var service = new ProductPromotionService(mockRepo.Object);

        await service.UpdateProductPromotionAsync(productPromotionToUpdate);

        mockRepo.Verify(repo => repo.UpdateAsync(productPromotionToUpdate), Times.Once());
    }
    
    [Fact]
    public async Task DeleteProductPromotionAsync_DeletesProductPromotion()
    {
        var mockRepo = new Mock<IRepository<ProductPromotion>>();
        var productPromotionToDelete = new ProductPromotion();

        var service = new ProductPromotionService(mockRepo.Object);

        await service.DeleteProductPromotionAsync(productPromotionToDelete);

        mockRepo.Verify(repo => repo.DeleteAsync(productPromotionToDelete), Times.Once());
    }
}