using DotNetCore.CAP;
using Microsoft.AspNetCore.Mvc;
using Order.Api.Domain.Events;
using Order.Api.Infrastructure;

namespace Order.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly ICapPublisher _capBus;
    private readonly OrderContext _context;

    public OrderController(
        ICapPublisher capPublisher,
        OrderContext context)
    {
        _context = context;
        _capBus = capPublisher;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] Domain.Model.Order order)
    {
        // Product price must be stored in the database instead of accepting value from requests
        foreach (var item in order.Items)
            order.TotalPrice = order.TotalPrice + (item.Price * item.Amount);

        _context.Orders.Add(order);

        await using (_context.Database.BeginTransaction(_capBus, true))
        {
            await _capBus.PublishAsync("order.created",
                new OrderCreated {Id = order.Id, Items = order.Items});

            await _context.SaveChangesAsync();
        }

        return Created("/order", order);
    }
}
