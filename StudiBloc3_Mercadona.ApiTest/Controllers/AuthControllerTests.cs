using Microsoft.AspNetCore.Mvc;
using StudiBloc3_Mercadona.Api.Controllers;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.ApiTest.Controllers;

public class AuthControllerTests
{
    [Fact]
    public void Login_WithCorrectCredentials_ReturnsOkWithToken()
    {
        var controller = new AuthController();
        var loginModel = new LoginModel { Username = "root", Password = "manager" };

        var result = controller.Login(loginModel);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.NotNull(okResult.Value);
    }
    
    [Fact]
    public void Login_WithIncorrectCredentials_ReturnsUnauthorized()
    {
        var controller = new AuthController();
        var loginModel = new LoginModel { Username = "user", Password = "password" };

        var result = controller.Login(loginModel);

        Assert.IsType<UnauthorizedResult>(result);
    }
}