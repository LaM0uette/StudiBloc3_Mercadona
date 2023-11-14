using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.Api.Controllers;

public class AuthController: ControllerBase
{
    [HttpPost("api/[controller]/Login")]
    public IActionResult Login([FromBody] LoginModel login)
    {
        if (login is not {Username: "root", Password: "manager"}) 
            return Unauthorized();
        
        var token = GenerateJwtToken(login.Username);
        return Ok(token);
    }

    private static string GenerateJwtToken(string username)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("DkDslmwwPYCdGqscpuHxUnPRS9QfpaiLqzPN0DOlkzE="));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var token = new JwtSecurityToken(
            issuer: "https://studi-mercadona.azurewebsites.net",
            audience: "https://studi-mercadona-api.azurewebsites.net",
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}