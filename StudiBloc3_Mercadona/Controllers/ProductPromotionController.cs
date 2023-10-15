using Microsoft.AspNetCore.Mvc;
using StudiBloc3_Mercadona.Core.Services;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.Controllers;

[ApiController]
[Route("api/[controller]/ProductPromotion")]
public class ProductPromotionController(IProductPromotionService productPromotionService) : ControllerBase
{
    #region Routes

    [HttpGet]
    [Route("GetAllProductPromotions")]
    public Task<IEnumerable<ProductPromotion>> GetAllProductPromotions()
    {
        return productPromotionService.GetAllProductPromotionsAsync();
    }

    [HttpPost]
    [Route("AddProductPromotion")]
    public IActionResult AddProductPromotion(ProductPromotion productPromotion)
    {
        productPromotionService.AddProductPromotionAsync(productPromotion);
        return Ok();
    }
    
    #endregion
}