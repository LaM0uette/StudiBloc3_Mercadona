using Microsoft.EntityFrameworkCore;
using StudiBloc3_Mercadona.Core.Context;
using StudiBloc3_Mercadona.Core.Repository;
using StudiBloc3_Mercadona.Core.Services;
using StudiBloc3_Mercadona.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// PostgreSQL connection
var Configuration = builder.Configuration;
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IRepository<Product>, Repository<Product>>();
builder.Services.AddTransient<IProductService, ProductService>();

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