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
    [Route("GetAll")]
    public Task<IEnumerable<Promotion>> GetAll()
    {
        return promotionService.GetAllPromotionsAsync();
    }

    [HttpPost]
    [Route("Add")]
    public IActionResult Add(Promotion promotion)
    {
        promotionService.AddPromotionAsync(promotion);
        return Ok();
    }
    
    #endregion
}