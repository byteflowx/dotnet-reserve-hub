using Microsoft.EntityFrameworkCore;
using ReserveHub.Domain.Bookings;
using ReserveHub.Domain.Resources;

namespace ReserveHub.Application.Common;

public interface IAppDbContext
{
    DbSet<Resource> Resources { get; }

    DbSet<Booking> Bookings { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
