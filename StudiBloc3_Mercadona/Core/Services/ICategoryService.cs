using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.Core.Services;

public interface ICategoryService
{
    Task<IEnumerable<Category>> GetAllCategoriesAsync();
    Task AddCategoryAsync(Category category);
}