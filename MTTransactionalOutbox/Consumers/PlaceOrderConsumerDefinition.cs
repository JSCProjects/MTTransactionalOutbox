using MassTransit;
using MTTransactionalOutbox.Infrastructure.Database;

namespace MTTransactionalOutbox.Consumers;

public class PlaceOrderConsumerDefinition
    : ConsumerDefinition<PlaceOrderConsumer>
{
    protected override void ConfigureConsumer(
        IReceiveEndpointConfigurator endpointConfigurator,
        IConsumerConfigurator<PlaceOrderConsumer> consumerConfigurator,
        IRegistrationContext context
    )
    {
        endpointConfigurator.UseEntityFrameworkOutbox<DemoDbContext>(context, configurator =>
        {
            configurator.MessageDeliveryLimit = 10;
        });
    }
}