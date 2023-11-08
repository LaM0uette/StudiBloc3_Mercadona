using System.Net.Http.Json;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.App.Services;

public class ApiCategoryService(HttpClient httpClient)
{
    public async Task<List<Category>> GetAllCategoriesAsync()
    {
        var response = await httpClient.GetAsync("/api/Category/Category/GetAll");
        response.EnsureSuccessStatusCode();
        
        var categories = await response.Content.ReadFromJsonAsync<List<Category>>();
        return categories ?? new List<Category>();
    }

    public async Task<Category?> AddCategoryAsync(Category category)
    {
        var response = await httpClient.PostAsJsonAsync("/api/Category/Category/Add", category);
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadFromJsonAsync<Category>();
    }
}