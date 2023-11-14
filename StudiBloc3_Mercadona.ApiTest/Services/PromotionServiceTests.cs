using Moq;
using StudiBloc3_Mercadona.Api.Core.Repository;
using StudiBloc3_Mercadona.Api.Core.Services;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.ApiTest.Services;

public class PromotionServiceTests
{
    [Fact]
    public async Task GetAllPromotionsAsync_ReturnsAllPromotions()
    {
        var mockRepo = new Mock<IRepository<Promotion>>();
        mockRepo.Setup(repo => repo.GetAllAsync())
            .ReturnsAsync(new List<Promotion> { new(), new() });

        var service = new PromotionService(mockRepo.Object);

        var result = await service.GetAllPromotionsAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }
    
    [Fact]
    public async Task AddPromotionAsync_AddsPromotionAndReturnsIt()
    {
        var promotionToAdd = new Promotion();
        var mockRepo = new Mock<IRepository<Promotion>>();
        mockRepo.Setup(repo => repo.AddAsync(It.IsAny<Promotion>()))
            .Callback<Promotion>(p => promotionToAdd = p);

        var service = new PromotionService(mockRepo.Object);

        var result = await service.AddPromotionAsync(new Promotion());

        Assert.NotNull(result);
        Assert.Equal(promotionToAdd, result);
    }
}