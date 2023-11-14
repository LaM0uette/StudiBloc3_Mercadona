using Microsoft.EntityFrameworkCore;
using StudiBloc3_Mercadona.Api.Core.Context;

namespace StudiBloc3_Mercadona.ApiTest.Context;

public class AppDbContextTests
{
    private AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public void AppDbContext_HasProductDbSet()
    {
        using var context = CreateDbContext();
        Assert.NotNull(context.Product);
    }

    [Fact]
    public void AppDbContext_HasCategoryDbSet()
    {
        using var context = CreateDbContext();
        Assert.NotNull(context.Category);
    }

    [Fact]
    public void AppDbContext_HasPromotionDbSet()
    {
        using var context = CreateDbContext();
        Assert.NotNull(context.Promotion);
    }

    [Fact]
    public void AppDbContext_HasProductPromotionDbSet()
    {
        using var context = CreateDbContext();
        Assert.NotNull(context.ProductPromotion);
    }
}