﻿using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.ServerApp.Services;

public class ApiProductService(HttpClient httpClient)
{
    public async Task<List<Product>> GetAllProductsAsync()
    {
        var response = await httpClient.GetAsync("/api/Product/Product/GetAll");
        response.EnsureSuccessStatusCode();

        var products = await response.Content.ReadFromJsonAsync<List<Product>>();
        return products ?? new List<Product>();
    }

    public async Task<Product> AddProductAsync(Product product)
    {
        var response = await httpClient.PostAsJsonAsync("/api/Product/Product/Add", product);
        response.EnsureSuccessStatusCode();

        var addedProduct = await response.Content.ReadFromJsonAsync<Product>();
        return addedProduct ?? new Product();
    }
}