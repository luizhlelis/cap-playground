using Microsoft.EntityFrameworkCore;
using Order.Api.Domain.Model;

namespace Order.Api.Infrastructure;

public class OrderContext : DbContext
{
    public OrderContext(DbContextOptions<OrderContext> options) : base(options)
    {
    }

    public DbSet<Domain.Model.Order> Orders { get; set; }

    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        Seed(modelBuilder);
    }

    private static void Seed(ModelBuilder modelBuilder)
    {
        // users
        var orderItems = new List<OrderItem>
        {
            new OrderItem {Id = Guid.Parse("301e53c1-a6cd-4ed4-a150-9e36ed0d40d7")}
        };
        modelBuilder.Entity<OrderItem>().HasData(orderItems);
    }
}
