using Microsoft.AspNetCore.Mvc;
using StudiBloc3_Mercadona.Api.Core.Services;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.Api.Controllers;

[ApiController]
[Route("api/[controller]/Category")]
public class CategoryController(ICategoryService categoryService) : ControllerBase
{
    #region Routes

    [HttpGet]
    [Route("GetAll")]
    public Task<IEnumerable<Category>> GetAll()
    {
        return categoryService.GetAllCategoriesAsync();
    }

    [HttpPost]
    [Route("Add")]
    public IActionResult Add(Category category)
    {
        categoryService.AddCategoryAsync(category);
        return Ok();
    }
    
    #endregion
}