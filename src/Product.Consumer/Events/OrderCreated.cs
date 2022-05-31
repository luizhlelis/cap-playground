using Ziggurat;

namespace Product.Consumer.Events;

public class OrderCreated : IMessage
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid Product { get; set; }
    public double TotalPrice { get; set; }

    public string MessageId { get; set; }

    public string MessageGroup { get; set; }
}
