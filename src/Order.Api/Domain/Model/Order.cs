namespace Order.Api.Domain.Model;

public class Order
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid Product { get; set; }
    public double TotalPrice { get; set; }
}
