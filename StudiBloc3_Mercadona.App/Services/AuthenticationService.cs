using System.Net.Http.Headers;
using Blazored.LocalStorage;

namespace StudiBloc3_Mercadona.App.Services;

public class AuthenticationService
{
    private readonly ILocalStorageService _localStorage;
    private readonly HttpClient _httpClient;

    public AuthenticationService(ILocalStorageService localStorage, HttpClient httpClient)
    {
        _localStorage = localStorage;
        _httpClient = httpClient;
    }

    public async Task<bool> Login(string username, string password)
    {
        var jwtToken = "jwt-token";

        if (username == "root" && password == "manager")
        {
            await _localStorage.SetItemAsync("jwtToken", jwtToken);
            return true;
        }

        return false;
    }

    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync("jwtToken");
    }

    public async Task<string> GetTokenAsync()
    {
        return await _localStorage.GetItemAsync<string>("jwtToken");
    }

    public async Task<bool> IsUserAuthenticated()
    {
        var token = await GetTokenAsync();
        return !string.IsNullOrWhiteSpace(token);
    }

    public async Task InitializeHttpClient()
    {
        var token = await GetTokenAsync();
        if (!string.IsNullOrWhiteSpace(token))
        {
            // Ajouter le token JWT à l'en-tête d'autorisation pour toutes les requêtes HTTP
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }
    }
}