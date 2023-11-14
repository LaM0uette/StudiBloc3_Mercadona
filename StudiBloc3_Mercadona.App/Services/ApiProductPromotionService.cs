using System.Net.Http.Json;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.App.Services;

public class ApiProductPromotionService(HttpClient httpClient)
{
    public async Task<List<ProductPromotion>> GetAllProductPromotionsAsync()
    {
        var response = await httpClient.GetAsync("/api/ProductPromotion/GetAll");
        response.EnsureSuccessStatusCode();
        
        var productPromotion = await response.Content.ReadFromJsonAsync<List<ProductPromotion>>();
        return productPromotion ?? new List<ProductPromotion>();
    }
    
    public async Task<ProductPromotion?> AddProductPromotionAsync(ProductPromotion productPromotion)
    {
        var response = await httpClient.PostAsJsonAsync("/api/ProductPromotion/Add", productPromotion);
        response.EnsureSuccessStatusCode();
        
        return await response.Content.ReadFromJsonAsync<ProductPromotion>();
    }
    
    public async Task UpdateProductPromotionAsync(ProductPromotion productPromotion)
    {
        var response = await httpClient.PutAsJsonAsync("/api/ProductPromotion/Update", productPromotion);
        response.EnsureSuccessStatusCode();
    }
    
    public async Task DeleteProductPromotionAsync(ProductPromotion productPromotion)
    {
        var response = await httpClient.PostAsJsonAsync("/api/ProductPromotion/Delete", productPromotion);
        response.EnsureSuccessStatusCode();
    }
}