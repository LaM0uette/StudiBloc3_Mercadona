﻿using Microsoft.AspNetCore.Mvc;
using StudiBloc3_Mercadona.Core.Services;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.Controllers;

[ApiController]
[Route("api/[controller]/Product")]
public class ProductController(IProductService _productService) : ControllerBase
{
    #region Routes

    [HttpGet]
    [Route("GetAllProducts")]
    public Task<IEnumerable<Product>> GetAllProducts()
    {
        return _productService.GetAllProductsAsync();
    }

    [HttpPost]
    [Route("AddProduct")]
    public IActionResult AddProduct(Product product)
    {
        _productService.AddProductAsync(product);
        return Ok();
    }
    
    #endregion
}