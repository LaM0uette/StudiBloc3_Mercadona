using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudiBloc3_Mercadona.Api.Core.Services;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController(ICategoryService categoryService) : ControllerBase
{
    [HttpGet]
    [Route("GetAll")]
    public async Task<IEnumerable<Category>> GetAll()
    {
        var categories = await categoryService.GetAllCategoriesAsync();
        return categories;
    }

    [HttpPost]
    [Authorize]
    [Route("Add")]
    public async Task<ActionResult<Category>> Add(Category category)
    {
        await categoryService.AddCategoryAsync(category);
        return Ok(category);
    }
}