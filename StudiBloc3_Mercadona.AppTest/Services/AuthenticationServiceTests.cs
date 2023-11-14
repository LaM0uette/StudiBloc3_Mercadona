using Blazored.LocalStorage;
using Moq;
using RichardSzalay.MockHttp;
using StudiBloc3_Mercadona.App.Services;

namespace StudiBloc3_Mercadona.AppTest.Services;

public class AuthenticationServiceTests
{
    private readonly MockHttpMessageHandler _mockHttp;
    private readonly HttpClient _mockHttpClient;
    private readonly Mock<ILocalStorageService> _mockLocalStorage;

    public AuthenticationServiceTests()
    {
        _mockHttp = new MockHttpMessageHandler();
        _mockHttpClient = new HttpClient(_mockHttp)
        {
            BaseAddress = new Uri("https://localhost:7173")
        };
        _mockLocalStorage = new Mock<ILocalStorageService>();
    }
    
    [Fact]
    public async Task Logout_RemovesTokenFromLocalStorage()
    {
        var service = new AuthenticationService(_mockLocalStorage.Object, _mockHttpClient);

        await service.Logout();

        _mockLocalStorage.Verify(storage => storage.RemoveItemAsync("jwtToken", It.IsAny<CancellationToken>()), Times.Once());
    }
    
    [Fact]
    public async Task IsUserAuthenticated_TokenExists_ReturnsTrue()
    {
        var token = "mockJwtToken";
        _mockLocalStorage.Setup(storage => storage.GetItemAsync<string>("jwtToken", It.IsAny<CancellationToken>()))
            .ReturnsAsync(token);

        var service = new AuthenticationService(_mockLocalStorage.Object, _mockHttpClient);

        var isAuthenticated = await service.IsUserAuthenticated();

        Assert.True(isAuthenticated);
    }
}