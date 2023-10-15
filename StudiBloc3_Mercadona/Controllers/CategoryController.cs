using Microsoft.AspNetCore.Mvc;
using StudiBloc3_Mercadona.Core.Services;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.Controllers;

[ApiController]
[Route("api/[controller]/Category")]
public class CategoryController(ICategoryService _categoryService) : ControllerBase
{
    #region Routes

    [HttpGet]
    [Route("GetAllCategories")]
    public Task<IEnumerable<Category>> GetAllCategories()
    {
        return _categoryService.GetAllCategoriesAsync();
    }

    [HttpPost]
    [Route("AddCategory")]
    public IActionResult AddCategory(Category category)
    {
        _categoryService.AddCategoryAsync(category);
        return Ok();
    }
    
    #endregion
}