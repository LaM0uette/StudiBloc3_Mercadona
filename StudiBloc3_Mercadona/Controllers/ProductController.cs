using Microsoft.AspNetCore.Mvc;
using StudiBloc3_Mercadona.Core.Services;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.Controllers;

[ApiController]
[Route("api/[controller]/Product")]
public class ProductController : ControllerBase
{
    #region Statements

    private readonly IProductService productService;

    public ProductController(IProductService _productService)
    {
        productService = _productService;
    }

    #endregion

    #region Routes

    [HttpGet]
    [Route("GetAllProducts")]
    public IEnumerable<Product> GetAllProducts()
    {
        return productService.GetAllProductsAsync().Result;
    }
    
    [HttpGet]
    [Route("GetProductById")]
    public Product GetProductById(int id)
    {
        return productService.GetProductByIdAsync(id).Result;
    }

    [HttpPost]
    [Route("AddProduct")]
    public IActionResult AddProduct(Product product)
    {
        productService.AddProductAsync(product);
        return Ok();
    }

    [HttpPut]
    [Route("UpdateProduct")]
    public IActionResult UpdateProduct(Product product)
    {
        productService.UpdateProductAsync(product);
        return Ok();
    }

    [HttpDelete]
    [Route("DeleteProduct")]
    public IActionResult DeleteProduct(int id)
    {
        var existingProduct = productService.GetProductByIdAsync(id).Result;
        productService.DeleteProductAsync(existingProduct);
        return Ok();
    }
    
    #endregion
}