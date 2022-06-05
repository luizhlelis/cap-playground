using System.ComponentModel.DataAnnotations.Schema;

namespace Order.Api.Domain.Model;

public class OrderItem
{
    public Guid Id { get; set; }

    public int ProductCode { get; set; }

    public double Price { get; set; }

    public int Amount { get; set; }
}
