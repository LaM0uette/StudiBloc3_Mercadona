﻿using StudiBloc3_Mercadona.Core.Repository;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.Core.Services;

public class ProductService(IRepository<Product> productRepository) : IProductService
{
    #region Tasks

    public Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return productRepository.GetAllAsync();
    }

    public Task AddProductAsync(Product product)
    {
        return productRepository.AddAsync(product);
    }

    #endregion
}