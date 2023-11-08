using Microsoft.AspNetCore.Mvc;
using StudiBloc3_Mercadona.Api.Core.Services;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.Api.Controllers;

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
    
    [HttpGet]
    [Route("GetIdByDiscountPercentage/{discountPercentage:int}")]
    public Task<int> GetPromotionIdByDiscountPercentageAsync(int discountPercentage)
    {
        return promotionService.GetPromotionIdByDiscountPercentageAsync(discountPercentage);
    }
 
    [HttpPost]
    [Route("Add")]
    public async Task<ActionResult<Promotion>> Add(Promotion promotion)
    {
        await promotionService.AddPromotionAsync(promotion);
        return Ok(promotion);
    }
    
    #endregion
}