using StudiBloc3_Mercadona.Api.Core.Repository;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.Api.Core.Services;

public class CategoryService(IRepository<Category> categoryRepository) : ICategoryService
{
    #region Tasks

    public Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return categoryRepository.GetAllAsync();
    }

    public async Task<Category> AddCategoryAsync(Category category)
    {
        await categoryRepository.AddAsync(category);
        return category; 
    }

    #endregion
}