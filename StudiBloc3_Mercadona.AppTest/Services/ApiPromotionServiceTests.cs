using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using StudiBloc3_Mercadona.App.Services;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.AppTest.Services;

public class ApiPromotionServiceTests
{
    private readonly MockHttpMessageHandler _mockHttp;
    private readonly HttpClient _mockHttpClient;

    public ApiPromotionServiceTests()
    {
        _mockHttp = new MockHttpMessageHandler();
        _mockHttpClient = new HttpClient(_mockHttp)
        {
            BaseAddress = new Uri("https://localhost:7173")
        };
    }

    [Fact]
    public async Task GetAllPromotionsAsync_ReturnsAllPromotions()
    {
        var promotions = new List<Promotion> { new(), new() };
        _mockHttp.When(HttpMethod.Get, "/api/Promotion/GetAll")
            .Respond("application/json", JsonConvert.SerializeObject(promotions));

        var service = new ApiPromotionService(_mockHttpClient);

        var result = await service.GetAllPromotionsAsync();

        Assert.Equal(2, result.Count);
    }
    
    [Fact]
    public async Task AddPromotionAsync_AddsPromotionAndReturnsIt()
    {
        var promotionToAdd = new Promotion();
        _mockHttp.When(HttpMethod.Post, "/api/Promotion/Add")
            .Respond("application/json", JsonConvert.SerializeObject(promotionToAdd));

        var service = new ApiPromotionService(_mockHttpClient);

        var result = await service.AddPromotionAsync(new Promotion());

        Assert.NotNull(result);
    }
}