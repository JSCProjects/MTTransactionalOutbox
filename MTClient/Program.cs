using System.Reflection;
using MassTransit;
using Messages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MTClient;

public class Program
{
    private static IHost _host;
    
    public static async Task Main(string[] args)
    {
        _host = Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, configurationBuilder) =>
            {
                configurationBuilder
                    .AddUserSecrets(Assembly.GetExecutingAssembly())
                    .AddEnvironmentVariables();
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddMassTransit(x =>
                {
                    x.UsingAzureServiceBus((context, cfg) =>
                    {
                        cfg.Host(hostContext.Configuration.GetConnectionString("ServiceBus"));
                    });
                });
            })
            .Build();
            
            await _host.StartAsync();
            
            await RunLoop();
    }
    
    private static readonly ILogger<Program> Logger =
        LoggerFactory.Create(builder =>
        {
            builder.AddConsole();
        }).CreateLogger<Program>();
    
    static async Task RunLoop()
    {
        using var scope = _host.Services.CreateScope();
        var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
        
        while (true)
        {
            Logger.LogInformation("Press 'P' to place an order, or 'Q' to quit.");
            var key = Console.ReadKey();
            Console.WriteLine();

            switch (key)
            {
                case { Key: ConsoleKey.P }:
                    var command = new PlaceOrder
                    {
                        OrderId = NewId.NextGuid().ToString()
                    };
                    
                    Logger.LogInformation("Sending PlaceOrder command, OrderId = {OrderId}", command.OrderId);
                    await publishEndpoint.Publish(command);

                    break;

                case { Key: ConsoleKey.Q }:
                    return;

                default:
                    Logger.LogInformation("Unknown input. Please try again.");
                    break;
            }
        }
    }
}