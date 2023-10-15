﻿using Microsoft.EntityFrameworkCore;
using StudiBloc3_Mercadona.Api.Model;

namespace StudiBloc3_Mercadona.Api.Core.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Product>? Product { get; set; }
    public DbSet<Category>? Category { get; set; }
    public DbSet<Promotion>? Promotion { get; set; }
    public DbSet<ProductPromotion>? ProductPromotion { get; set; }
}