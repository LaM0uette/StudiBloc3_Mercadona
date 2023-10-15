using Microsoft.EntityFrameworkCore;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.Core.Context;

public class RepositoryDbContext : DbContext
{
    public DbSet<Product>? Product { get; set; }
    public DbSet<Category>? Category { get; set; }
    
    public RepositoryDbContext(DbContextOptions<RepositoryDbContext> options) : base(options)
    {
    }
}