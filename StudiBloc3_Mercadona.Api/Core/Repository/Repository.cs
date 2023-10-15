using Microsoft.EntityFrameworkCore;
using StudiBloc3_Mercadona.Api.Core.Context;

namespace StudiBloc3_Mercadona.Api.Core.Repository;

public class Repository<T>(AppDbContext dbContext) : IRepository<T> where T : class
{
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await dbContext.Set<T>().ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await dbContext.Set<T>().AddAsync(entity);
        await dbContext.SaveChangesAsync();
    }
}