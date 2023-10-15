using System.Net.Http.Json;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.WebApp.Services;

public class ApiProductService(HttpClient httpClient)
{
    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        var response = await httpClient.GetAsync("/api/Product/Product/GetAll");
        response.EnsureSuccessStatusCode();
        
        var products = await response.Content.ReadFromJsonAsync<IEnumerable<Product>>();
        return products ?? Array.Empty<Product>();
    }
}