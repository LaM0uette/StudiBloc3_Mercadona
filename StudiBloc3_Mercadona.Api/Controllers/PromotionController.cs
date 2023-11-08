using Microsoft.AspNetCore.Mvc;
using StudiBloc3_Mercadona.Api.Core.Services;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PromotionController(IPromotionService promotionService) : ControllerBase
{
    [HttpGet]
    [Route("GetAll")]
    public async Task<IEnumerable<Promotion>> GetAll()
    {
        var promotions = await promotionService.GetAllPromotionsAsync();
        return promotions;
    }
 
    [HttpPost]
    [Route("Add")]
    public async Task<ActionResult<Promotion>> Add(Promotion promotion)
    {
        await promotionService.AddPromotionAsync(promotion);
        return Ok(promotion);
    }
}