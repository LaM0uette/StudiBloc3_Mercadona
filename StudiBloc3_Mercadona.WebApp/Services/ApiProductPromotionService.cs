using System.Net.Http.Json;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.WebApp.Services;

public class ApiProductPromotionService(HttpClient httpClient)
{
    public async Task<IEnumerable<ProductPromotion>> GetAllProductPromotionsAsync()
    {
        var response = await httpClient.GetAsync("/api/ProductPromotion/ProductPromotion/GetAll");
        response.EnsureSuccessStatusCode();
        
        var productPromotion = await response.Content.ReadFromJsonAsync<IEnumerable<ProductPromotion>>();
        return productPromotion ?? Array.Empty<ProductPromotion>();
    }
    
    public async Task AddProductPromotionAsync(ProductPromotion productPromotion)
    {
        var response = await httpClient.PostAsJsonAsync("/api/ProductPromotion/ProductPromotion/Add", productPromotion);
        response.EnsureSuccessStatusCode();
    }
}