using System.Net.Http.Headers;
using System.Net.Http.Json;
using Blazored.LocalStorage;

namespace StudiBloc3_Mercadona.App.Services;

public class AuthenticationService(ILocalStorageService localStorage, HttpClient httpClient)
{
    public async Task<bool> Login(string username, string password)
    {
        var response = await httpClient.PostAsJsonAsync("api/Auth/Login", new { username, password });
        if (!response.IsSuccessStatusCode)
        {
            Console.WriteLine("Login failed\n" + response.StatusCode);
            return false;
        }

        var token = await response.Content.ReadAsStringAsync();
        Console.WriteLine("Login successful - " + token);
        await localStorage.SetItemAsync("jwtToken", token);
        await InitializeHttpClient(token);

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