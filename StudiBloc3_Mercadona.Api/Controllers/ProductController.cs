using Microsoft.AspNetCore.Mvc;
using StudiBloc3_Mercadona.Api.Core.Services;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.Api.Controllers;

[ApiController]
[Route("api/[controller]/Product")]
public class ProductController(IProductService productService) : ControllerBase
{
    #region Routes

    [HttpGet]
    [Route("GetAll")]
    public Task<IEnumerable<Product>> GetAll()
    {
        return productService.GetAllProductsAsync();
    }
    
    [HttpPost]
    [Route("Add")]
    public async Task<ActionResult<Product>> Add(Product product)
    {
        await productService.AddProductAsync(product);
        return Ok(product);
    }
    
    #endregion
}