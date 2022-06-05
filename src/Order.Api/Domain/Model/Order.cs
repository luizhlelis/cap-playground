namespace Order.Api.Domain.Model;

public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime CreationDate { get; set; } = DateTime.Now;

    public List<OrderItem> Items { get; set; } = new List<OrderItem>();

    public double TotalPrice { get; set; }
}
