namespace StudiBloc3_Mercadona.Api.Core.Repository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
    }
}