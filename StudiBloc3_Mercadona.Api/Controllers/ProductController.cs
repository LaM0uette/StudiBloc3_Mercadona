using Microsoft.AspNetCore.Mvc;
using StudiBloc3_Mercadona.Api.Core.Services;
using StudiBloc3_Mercadona.Api.Model;

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
    public IActionResult Add(Product product)
    {
        productService.AddProductAsync(product);
        return Ok();
    }
    
    #endregion
}