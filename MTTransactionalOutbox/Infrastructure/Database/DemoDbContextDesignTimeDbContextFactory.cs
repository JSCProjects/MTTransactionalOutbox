using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MTTransactionalOutbox.Infrastructure.Database;

public class DemoDbContextDesignTimeDbContextFactory : IDesignTimeDbContextFactory<DemoDbContext>
{
    public DemoDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<DemoDbContext>();
        optionsBuilder.UseSqlServer(
            @"Server=(localdb)\mssqllocaldb;Database=DemoDb;ConnectRetryCount=0"
        );
        return new DemoDbContext(optionsBuilder.Options);
    }
}