namespace Catalog.Consumer.Idempotency;

public interface IMessage
{
    string MessageId { get; set; }

    string MessageGroup { get; set; }
}
