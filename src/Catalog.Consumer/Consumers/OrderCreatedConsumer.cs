using Catalog.Consumer.Domain.Events;
using Catalog.Consumer.Idempotency;
using DotNetCore.CAP;

namespace Catalog.Consumer.Consumers;

public class OrderCreatedConsumer : ICapSubscribe
{
    private readonly ILogger<OrderCreatedConsumer> _logger;
    private readonly IConsumerService<OrderCreated> _service;

    public OrderCreatedConsumer(ILogger<OrderCreatedConsumer> logger,
        IConsumerService<OrderCreated> service)
    {
        _logger = logger;
        _service = service;
    }

    [CapSubscribe("order.created", Group = "catalog.order.created")]
    public async Task UpdateProductStock(OrderCreated message)
    {
        _logger.LogInformation("Decrease stock");
        await _service.ProcessMessageAsync(message);
    }
}
