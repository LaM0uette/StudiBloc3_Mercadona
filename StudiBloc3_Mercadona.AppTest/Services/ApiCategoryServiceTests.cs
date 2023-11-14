using Newtonsoft.Json;
using RichardSzalay.MockHttp;
using StudiBloc3_Mercadona.App.Services;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.AppTest.Services;

public class ApiCategoryServiceTests
{
    private readonly MockHttpMessageHandler _mockHttp;
    private readonly HttpClient _mockHttpClient;

    public ApiCategoryServiceTests()
    {
        _mockHttp = new MockHttpMessageHandler();

        _mockHttpClient = new HttpClient(_mockHttp)
        {
            BaseAddress = new Uri("https://localhost:7173")
        };
    }

    [Fact]
    public async Task GetAllCategoriesAsync_ReturnsAllCategories()
    {
        var categories = new List<Category> { new(), new() };
        _mockHttp.When(HttpMethod.Get, "/api/Category/GetAll")
            .Respond("application/json", JsonConvert.SerializeObject(categories));

        var service = new ApiCategoryService(_mockHttpClient);

        var result = await service.GetAllCategoriesAsync();

        Assert.Equal(2, result.Count);
    }
    
    [Fact]
    public async Task AddCategoryAsync_AddsCategoryAndReturnsIt()
    {
        var newCategory = new Category();
        _mockHttp.When(HttpMethod.Post, "/api/Category/Add")
            .Respond("application/json", JsonConvert.SerializeObject(newCategory));

        var service = new ApiCategoryService(_mockHttpClient);

        var result = await service.AddCategoryAsync(new Category());

        Assert.NotNull(result);
    }
}