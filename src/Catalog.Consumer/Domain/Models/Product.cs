using System.ComponentModel.DataAnnotations;

namespace Catalog.Consumer.Domain.Models;

public class Product
{
    [Key] public int Code { get; set; }

    public string Name { get; set; }

    public string Description { get; set; }

    public double Price { get; set; }

    public int AmountAvailable { get; set; }
}
