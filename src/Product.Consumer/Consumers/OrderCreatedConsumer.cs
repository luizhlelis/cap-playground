using DotNetCore.CAP;
using Product.Consumer.Events;

namespace Product.Consumer.Consumers;

public class OrderCreatedConsumer : ICapSubscribe
{
    private readonly ILogger<OrderCreatedConsumer> _logger;

    public OrderCreatedConsumer(ILogger<OrderCreatedConsumer> logger)
    {
        _logger = logger;
    }

    [CapSubscribe("order.created", Group = "product.order.canceled")]
    public Task UpdateOrderStockAsync(OrderCreated message)
    {
        _logger.LogInformation("Restore stock");
        return Task.CompletedTask;
    }
}
