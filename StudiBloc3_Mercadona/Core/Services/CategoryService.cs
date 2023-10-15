﻿using StudiBloc3_Mercadona.Core.Repository;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.Core.Services;

public class CategoryService(IRepository<Category> categoryRepository) : ICategoryService
{
    #region Tasks

    public Task<IEnumerable<Category>> GetAllCategoriesAsync()
    {
        return categoryRepository.GetAllAsync();
    }

    public Task AddCategoryAsync(Category category)
    {
        return categoryRepository.AddAsync(category);
    }

    #endregion
}