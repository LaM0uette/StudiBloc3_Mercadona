﻿using Microsoft.AspNetCore.Mvc;
using StudiBloc3_Mercadona.Api.Core.Services;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductPromotionController(IProductPromotionService productPromotionService) : ControllerBase
{
    [HttpGet]
    [Route("GetAll")]
    public async Task<IEnumerable<ProductPromotion>> GetAll()
    {
        var productPromotions = await productPromotionService.GetAllProductPromotionsAsync();
        return productPromotions;
    }

    [HttpPost]
    [Route("Add")]
    public async Task<ActionResult<ProductPromotion>> Add(ProductPromotion productPromotion)
    {
        await productPromotionService.AddProductPromotionAsync(productPromotion);
        return Ok(productPromotion);
    }

    [HttpPut]
    [Route("Update")]
    public async Task<IActionResult> Update(ProductPromotion productPromotion)
    {
        await productPromotionService.UpdateProductPromotionAsync(productPromotion);
        return Ok();
    }

    [HttpPost]
    [Route("Delete")]
    public async Task<IActionResult> Delete(ProductPromotion productPromotion)
    {
        await productPromotionService.DeleteProductPromotionAsync(productPromotion);
        return Ok();
    }
}