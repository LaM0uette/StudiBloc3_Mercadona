using Microsoft.AspNetCore.Mvc;
using StudiBloc3_Mercadona.Core.Services;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.Controllers;

[ApiController]
[Route("api/[controller]/Promotion")]
public class PromotionController(IPromotionService promotionService) : ControllerBase
{
    #region Routes

    [HttpGet]
    [Route("GetAllPromotions")]
    public Task<IEnumerable<Promotion>> GetAllPromotions()
    {
        return promotionService.GetAllPromotionsAsync();
    }

    [HttpPost]
    [Route("AddPromotion")]
    public IActionResult AddPromotion(Promotion promotion)
    {
        promotionService.AddPromotionAsync(promotion);
        return Ok();
    }
    
    #endregion
}