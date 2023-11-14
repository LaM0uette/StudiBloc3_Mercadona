using System.Net;
using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using StudiBloc3_Mercadona.App.Services;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.AppTest.Services;

public class ApiProductPromotionServiceTests
{
    private readonly MockHttpMessageHandler _mockHttp;
    private readonly HttpClient _mockHttpClient;

    public ApiProductPromotionServiceTests()
    {
        _mockHttp = new MockHttpMessageHandler();
        _mockHttpClient = new HttpClient(_mockHttp)
        {
            BaseAddress = new Uri("https://localhost:7173")
        };
    }

    [Fact]
    public async Task GetAllProductPromotionsAsync_ReturnsAllProductPromotions()
    {
        var productPromotions = new List<ProductPromotion> { new(), new() };
        _mockHttp.When(HttpMethod.Get, "/api/ProductPromotion/GetAll")
            .Respond("application/json", JsonConvert.SerializeObject(productPromotions));

        var service = new ApiProductPromotionService(_mockHttpClient);

        var result = await service.GetAllProductPromotionsAsync();

        Assert.Equal(2, result.Count);
    }
    
    [Fact]
    public async Task AddProductPromotionAsync_AddsProductPromotionAndReturnsIt()
    {
        var productPromotionToAdd = new ProductPromotion();
        _mockHttp.When(HttpMethod.Post, "/api/ProductPromotion/Add")
            .Respond("application/json", JsonConvert.SerializeObject(productPromotionToAdd));

        var service = new ApiProductPromotionService(_mockHttpClient);

        var result = await service.AddProductPromotionAsync(new ProductPromotion());

        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task UpdateProductPromotionAsync_UpdatesProductPromotion()
    {
        _mockHttp.When(HttpMethod.Put, "/api/ProductPromotion/Update")
            .Respond(HttpStatusCode.OK);

        var service = new ApiProductPromotionService(_mockHttpClient);

        await service.UpdateProductPromotionAsync(new ProductPromotion());
    }
    
    [Fact]
    public async Task DeleteProductPromotionAsync_DeletesProductPromotion()
    {
        _mockHttp.When(HttpMethod.Post, "/api/ProductPromotion/Delete")
            .Respond(HttpStatusCode.OK);

        var service = new ApiProductPromotionService(_mockHttpClient);

        await service.DeleteProductPromotionAsync(new ProductPromotion());
    }
}