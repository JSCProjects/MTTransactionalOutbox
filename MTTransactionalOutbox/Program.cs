using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using MTTransactionalOutbox.Infrastructure.Database;
using MTTransactionalOutbox.Infrastructure.MassTransit;

var builder = Host
    .CreateApplicationBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>();

builder
    .ConfigureDatabase()
    .ConfigureMassTransit();

var debug = builder.Configuration.GetDebugView();
Console.WriteLine(debug);

var host = builder.Build();
await host.RunAsync();