using Catalog.Consumer.Domain.Models;
using Catalog.Consumer.Idempotency;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Consumer.Infrastructure;

public class CatalogContext : DbContext
{
    public DbSet<Product> Products { get; set; }

    public DbSet<MessageTracking> Messages { get; set; }

    public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.MapMessageTracker();
        base.OnModelCreating(modelBuilder);

        Seed(modelBuilder);
    }

    private static void Seed(ModelBuilder modelBuilder)
    {
        // products
        var products = new List<Product>
        {
            new()
            {
                Code = 1234,
                Name = "T Shirt",
                Description = "White t shirt",
                Price = 5,
                AmountAvailable = 25
            },
            new()
            {
                Code = 5678,
                Name = "Jeans",
                Description = "Blue Jeans",
                Price = 10,
                AmountAvailable = 30
            }
        };
        modelBuilder.Entity<Product>().HasData(products);
    }
}
