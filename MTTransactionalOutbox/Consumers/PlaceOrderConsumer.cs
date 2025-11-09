using MassTransit;
using Messages;
using Microsoft.Extensions.Logging;

namespace MTTransactionalOutbox.Consumers;

public class PlaceOrderConsumer(ILogger<PlaceOrderConsumer> logger) : IConsumer<PlaceOrder>
{
    public async Task Consume(ConsumeContext<PlaceOrder> context)
    {
        logger.LogInformation("Received PlaceOrder, OrderId = {OrderId}", context.Message.OrderId);

        var events = new List<OrderPlaced>();
        for (var i = 0; i < context.Message.Amount; i++)
        {
            events.Add(new OrderPlaced
            {
                OrderId = context.Message.OrderId,
                PlaceId = i + 1
            });
        }
        
        var queue = new Uri("queue:orderplaced");
        
        await Task.WhenAll(events.Select(x => context.Send(queue, x))); }
}