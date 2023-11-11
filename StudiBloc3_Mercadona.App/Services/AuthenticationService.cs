using System.Net.Http.Headers;
using Blazored.LocalStorage;

namespace StudiBloc3_Mercadona.App.Services;

public class AuthenticationService(ILocalStorageService localStorage, HttpClient httpClient)
{
    public async Task<bool> Login(string username, string password)
    {
        const string jwtToken = "jwt-token";

        if (username != "root" || password != "manager") return false;
        
        await localStorage.SetItemAsync("jwtToken", jwtToken);
        await InitializeHttpClient(jwtToken);
        
        return true;
    }

    private Task InitializeHttpClient(string token)
    {
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return Task.CompletedTask;
    }

    public async Task Logout()
    {
        await localStorage.RemoveItemAsync("jwtToken");
        httpClient.DefaultRequestHeaders.Authorization = null;
    }

    public async Task<bool> IsUserAuthenticated()
    {
        var token = await localStorage.GetItemAsync<string>("jwtToken");
        return !string.IsNullOrWhiteSpace(token);
    }
}