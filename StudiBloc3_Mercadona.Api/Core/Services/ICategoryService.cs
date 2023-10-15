using StudiBloc3_Mercadona.Api.Model;

namespace StudiBloc3_Mercadona.Api.Core.Services;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
    Task AddCategoryAsync(Category category);
}