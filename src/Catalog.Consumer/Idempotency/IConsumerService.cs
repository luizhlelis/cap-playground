namespace Catalog.Consumer.Idempotency;

public interface IConsumerService<in TMessage> where TMessage : IMessage
{
    Task ProcessMessageAsync(TMessage message);
}
