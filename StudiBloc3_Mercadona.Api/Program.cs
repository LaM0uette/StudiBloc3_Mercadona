using Microsoft.EntityFrameworkCore;
using StudiBloc3_Mercadona.Api.Core.Context;
using StudiBloc3_Mercadona.Api.Core.Repository;
using StudiBloc3_Mercadona.Api.Core.Services;
using StudiBloc3_Mercadona.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// PostgreSQL connection and related Services and Repositories
var Configuration = builder.Configuration;
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IRepository<Product>, Repository<Product>>();
builder.Services.AddTransient<IProductService, ProductService>();

// Add Repositories
builder.Services.AddScoped<IRepository<Category>, Repository<Category>>();
builder.Services.AddScoped<IRepository<Promotion>, Repository<Promotion>>();
builder.Services.AddScoped<IRepository<ProductPromotion>, Repository<ProductPromotion>>();

// Add Services
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<IPromotionService, PromotionService>();
builder.Services.AddTransient<IProductPromotionService, ProductPromotionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();