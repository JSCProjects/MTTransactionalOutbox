using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MTTransactionalOutbox.Infrastructure.Database;

public static class DatabaseConfiguration
{
    public static IHostApplicationBuilder ConfigureDatabase(this IHostApplicationBuilder builder)
    {
        builder.Services.AddDbContext<DemoDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("Database"))
        );

        return builder;
    }
}
