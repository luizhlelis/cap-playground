using Product.Consumer.Events;
using Product.Consumer.Infrastructure;
using Ziggurat;

namespace Product.Consumer.Services;

public class OrderCreatedService : IConsumerService<OrderCreated>
{
    private readonly ProductContext _context;

    public OrderCreatedService(ProductContext context)
    {
        _context = context;
    }

    public async Task ProcessMessageAsync(OrderCreated message)
    {
        // Decrease product stock

        await _context.SaveChangesAsync();
    }
}
