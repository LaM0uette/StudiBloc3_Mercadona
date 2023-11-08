using Microsoft.AspNetCore.Mvc;
using StudiBloc3_Mercadona.Api.Core.Services;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(IProductService productService) : ControllerBase
{
    [HttpGet]
    [Route("GetAll")]
    public async Task<IEnumerable<Product>> GetAll()
    {
        var products = await productService.GetAllProductsAsync();
        return products;
    }
    
    [HttpPost]
    [Route("Add")]
    public async Task<ActionResult<Product>> Add(Product product)
    {
        await productService.AddProductAsync(product);
        return Ok(product);
    }
}