using Catalog.Consumer.Domain.Events;
using Catalog.Consumer.Idempotency;
using Catalog.Consumer.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Consumer.Domain.Services;

public class OrderCreatedService : IConsumerService<OrderCreated>
{
    private readonly CatalogContext _context;

    public OrderCreatedService(CatalogContext context)
    {
        _context = context;
    }

    public async Task ProcessMessageAsync(OrderCreated order)
    {
        var productCodes = order.Items.Select(x => x.ProductCode);
        var productAmount =
            order.Items.ToDictionary(x => x.ProductCode, x => x.Amount);

        var productsToUpdate = await _context.Products
            .Where(x => productCodes.Contains(x.Code))
            .ToListAsync();

        foreach (var product in productsToUpdate)
        {
            product.AmountAvailable -= productAmount[product.Code];
        }

        await _context.SaveChangesAsync();
    }
}
