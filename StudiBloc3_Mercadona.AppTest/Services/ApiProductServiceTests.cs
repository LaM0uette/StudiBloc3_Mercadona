using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using StudiBloc3_Mercadona.App.Services;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.AppTest.Services;

public class ApiProductServiceTests
{
    private readonly MockHttpMessageHandler _mockHttp;
    private readonly HttpClient _mockHttpClient;

    public ApiProductServiceTests()
    {
        _mockHttp = new MockHttpMessageHandler();
        _mockHttpClient = new HttpClient(_mockHttp)
        {
            BaseAddress = new Uri("https://localhost:7173")
        };
    }

    [Fact]
    public async Task GetAllProductsAsync_ReturnsAllProducts()
    {
        var products = new List<Product> { new(), new() };
        _mockHttp.When(HttpMethod.Get, "/api/Product/GetAll")
            .Respond("application/json", JsonConvert.SerializeObject(products));

        var service = new ApiProductService(_mockHttpClient);

        var result = await service.GetAllProductsAsync();

        Assert.Equal(2, result.Count);
    }
    
    [Fact]
    public async Task AddProductAsync_AddsProductAndReturnsIt()
    {
        var productToAdd = new Product();
        _mockHttp.When(HttpMethod.Post, "/api/Product/Add")
            .Respond("application/json", JsonConvert.SerializeObject(productToAdd));

        var service = new ApiProductService(_mockHttpClient);

        var result = await service.AddProductAsync(new Product());

        Assert.NotNull(result);
    }
}