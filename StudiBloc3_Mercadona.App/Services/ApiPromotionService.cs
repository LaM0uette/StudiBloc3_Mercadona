using System.Net.Http.Json;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.App.Services;

public class ApiPromotionService(HttpClient httpClient)
{
    public async Task<List<Promotion>> GetAllPromotionsAsync()
    {
        var response = await httpClient.GetAsync("/api/Promotion/GetAll");
        response.EnsureSuccessStatusCode();
        
        var promotion = await response.Content.ReadFromJsonAsync<List<Promotion>>();
        return promotion ?? new List<Promotion>();
    }
    
    public async Task<Promotion?> AddPromotionAsync(Promotion promotion)
    {
        var response = await httpClient.PostAsJsonAsync("/api/Promotion/Add", promotion);
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadFromJsonAsync<Promotion>();
    }
}