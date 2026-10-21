using Microsoft.EntityFrameworkCore;
using Npgsql;
using ReserveHub.Application.Bookings;
using ReserveHub.Application.Common;
using ReserveHub.Domain.Bookings;
using ReserveHub.Domain.Resources;

namespace ReserveHub.Infrastructure.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IAppDbContext
{
    public DbSet<Resource> Resources => Set<Resource>();

    public DbSet<Booking> Bookings => Set<Booking>();

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception)
            when (exception.InnerException is PostgresException { SqlState: PostgresErrorCodes.ExclusionViolation })
        {
            throw new BookingOverlapException(exception);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("btree_gist");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
