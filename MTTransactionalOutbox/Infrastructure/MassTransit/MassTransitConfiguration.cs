using System.Reflection;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MTTransactionalOutbox.Infrastructure.Database;

namespace MTTransactionalOutbox.Infrastructure.MassTransit;

public static class MassTransitConfiguration
{
    public static IHostApplicationBuilder ConfigureMassTransit(this IHostApplicationBuilder builder)
    {
        var services = builder.Services;

        var configuration = builder.Configuration;
        var messageHandlingAssembly = Assembly.GetEntryAssembly();

        services.AddMassTransit(x =>
        {
            x.AddServiceBusMessageScheduler();
            x.AddEntityFrameworkOutbox<DemoDbContext>(cfg =>
            {
                cfg.UseSqlServer();
                cfg.DuplicateDetectionWindow = TimeSpan.FromDays(14);
            });
            x.AddInMemoryInboxOutbox();
           
            x.AddConfigureEndpointsCallback(
                (registrationContext, _, configurator) =>
                {
                    if (configurator is not IServiceBusReceiveEndpointConfigurator cfg)
                    {
                        return;
                    }

                    // By default, use the in memory inbox outbox. Only for specific consumers use the EF Core outbox.
                    // And this causes the EF Core outbox to flush messages before all are dispatched to broker.
                    cfg.UseInMemoryInboxOutbox(registrationContext);
                    cfg.UseJsonSerializer();

                    cfg.LockDuration = TimeSpan.FromSeconds(60);

                    cfg.ConfigureDeadLetterQueueDeadLetterTransport();
                    cfg.ConfigureDeadLetterQueueErrorTransport();
                }
            );

            x.AddConsumers(messageHandlingAssembly);

            x.UsingAzureServiceBus(
                (context, configure) =>
                {
                    configure.UseJsonSerializer();
                    
                    configure.UseServiceBusMessageScheduler();
                    
                    configure.ConfigureEndpoints(
                        context,
                        new DefaultEndpointNameFormatter($"{Assembly.GetEntryAssembly().GetName().Name}." )
                    );
                    
                    configure.Host(configuration.GetConnectionString("ServiceBus"));
                }
            );
        });

        return builder;
    }
}
