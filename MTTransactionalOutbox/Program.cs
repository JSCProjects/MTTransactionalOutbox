using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MTTransactionalOutbox.Infrastructure.Database;
using MTTransactionalOutbox.Infrastructure.Logging;
using MTTransactionalOutbox.Infrastructure.MassTransit;

var builder = Host
    .CreateApplicationBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddProvider(new FileLoggerProvider("app.log"));

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>();

builder
    .ConfigureDatabase()
    .ConfigureMassTransit();

var host = builder.Build();
await host.RunAsync();