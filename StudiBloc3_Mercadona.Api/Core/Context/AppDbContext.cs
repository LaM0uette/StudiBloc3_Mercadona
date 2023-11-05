using Microsoft.EntityFrameworkCore;
using StudiBloc3_Mercadona.Model;

namespace StudiBloc3_Mercadona.Api.Core.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product>? Product { get; init; }
    public DbSet<Category>? Category { get; init; }
    public DbSet<Promotion>? Promotion { get; init; }
    public DbSet<ProductPromotion>? ProductPromotion { get; init; }
}