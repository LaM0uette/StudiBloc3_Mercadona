using Microsoft.AspNetCore.Mvc;
using StudiBloc3_Mercadona.Core.Services;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.Controllers;

[ApiController]
[Route("api/[controller]/Promotion")]
public class PromotionController(IPromotionService _promotionService) : ControllerBase
{
    #region Routes

    [HttpGet]
    [Route("GetAllPromotions")]
    public Task<IEnumerable<Promotion>> GetAllPromotions()
    {
        return _promotionService.GetAllPromotionsAsync();
    }

    [HttpPost]
    [Route("AddPromotion")]
    public IActionResult AddPromotion(Promotion promotion)
    {
        _promotionService.AddPromotionAsync(promotion);
        return Ok();
    }
    
    #endregion
}