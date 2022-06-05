using Order.Api.Domain.Model;

namespace Order.Api.Domain.Events;

public class OrderCreated
{
    public Guid Id { get; set; }

    public List<OrderItem> Items { get; set; }
}
