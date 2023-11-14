using StudiBloc3_Mercadona.Api.Core.Repository;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.Api.Core.Services;

public class CategoryService(IRepository<Category> categoryRepository) : ICategoryService
{
    public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        var categories = await categoryRepository.GetAllAsync();
        return categories;
    }

    public async Task<Category> AddCategoryAsync(Category category)
    {
        await categoryRepository.AddAsync(category);
        return category; 
    }
}