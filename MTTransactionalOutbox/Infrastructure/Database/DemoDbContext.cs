using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;

namespace MTTransactionalOutbox.Infrastructure.Database;

public class DemoDbContext(DbContextOptions<DemoDbContext> options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        modelBuilder.AddInboxStateEntity(x => x.ToTable(nameof(InboxState), "masstransit"));
        modelBuilder.AddOutboxStateEntity(x => x.ToTable(nameof(OutboxState), "masstransit"));
        modelBuilder.AddOutboxMessageEntity(x => x.ToTable(nameof(OutboxMessage), "masstransit"));
    }
}