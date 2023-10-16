using System.Net.Http.Json;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.WebApp.Services;

public class ApiCategoryService(HttpClient httpClient)
{
    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        var response = await httpClient.GetAsync("/api/Category/Category/GetAll");
        response.EnsureSuccessStatusCode();
        
        var categories = await response.Content.ReadFromJsonAsync<IEnumerable<Category>>();
        return categories ?? Array.Empty<Category>();
    }

    public async Task AddCategoryAsync(Category category)
    {
        var response = await httpClient.PostAsJsonAsync("/api/Category/Category/Add", category);
        response.EnsureSuccessStatusCode();
    }
}