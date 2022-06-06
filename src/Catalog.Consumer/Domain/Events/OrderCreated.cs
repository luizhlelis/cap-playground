using Catalog.Consumer.Idempotency;

namespace Catalog.Consumer.Domain.Events;

public class OrderCreated : IMessage
{
    public Guid Id { get; set; }

    public List<OrderItem> Items { get; set; }

    public string MessageId { get; set; }

    public string MessageGroup { get; set; }
}

public class OrderItem
{
    public Guid Id { get; set; }

    public int ProductCode { get; set; }

    public double Price { get; set; }

    public int Amount { get; set; }
}
