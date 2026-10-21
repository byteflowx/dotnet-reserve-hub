using Microsoft.EntityFrameworkCore;
using ReserveHub.Application.Bookings;
using ReserveHub.Application.Common;
using ReserveHub.Domain.Bookings;
using ReserveHub.Domain.Common;
using ReserveHub.Domain.Resources;

namespace ReserveHub.Application.Resources;

public sealed class GetAvailability(IAppDbContext db)
{
    public async Task<Result<AvailabilityDto>> HandleAsync(
        Guid resourceId, DateTimeOffset start, DateTimeOffset end, CancellationToken cancellationToken)
    {
        var periodResult = TimeRange.Create(start, end);
        if (periodResult.IsFailure)
        {
            return periodResult.Error;
        }

        var period = periodResult.Value;

        var resourceExists = await db.Resources.AnyAsync(r => r.Id == resourceId, cancellationToken);
        if (!resourceExists)
        {
            return ResourceErrors.NotFound;
        }

        var busySlots = await db.Bookings
            .AsNoTracking()
            .ConfirmedOverlapping(resourceId, period)
            .OrderBy(b => b.Start)
            .Select(b => new TimeSlotDto(b.Start, b.End))
            .ToListAsync(cancellationToken);

        return new AvailabilityDto(resourceId, period.Start, period.End, busySlots.Count == 0, busySlots);
    }
}
