using System.Net.Http.Json;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.WebApp.Services;

public class ApiPromotionService(HttpClient httpClient)
{
    public async Task<IEnumerable<Promotion>> GetAllPromotionsAsync()
    {
        var response = await httpClient.GetAsync("/api/Promotion/Promotion/GetAll");
        response.EnsureSuccessStatusCode();
        
        var promotion = await response.Content.ReadFromJsonAsync<IEnumerable<Promotion>>();
        return promotion ?? Array.Empty<Promotion>();
    }
}