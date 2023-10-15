using Microsoft.AspNetCore.Mvc;
using StudiBloc3_Mercadona.Api.Core.Services;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.Api.Controllers;

[ApiController]
[Route("api/[controller]/ProductPromotion")]
public class ProductPromotionController(IProductPromotionService productPromotionService) : ControllerBase
{
    #region Routes

    [HttpGet]
    [Route("GetAll")]
    public Task<IEnumerable<ProductPromotion>> GetAll()
    {
        return productPromotionService.GetAllProductPromotionsAsync();
    }

    [HttpPost]
    [Route("Add")]
    public IActionResult Add(ProductPromotion productPromotion)
    {
        productPromotionService.AddProductPromotionAsync(productPromotion);
        return Ok();
    }
    
    #endregion
}