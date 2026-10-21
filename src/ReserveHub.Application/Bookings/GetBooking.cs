using Microsoft.EntityFrameworkCore;
using ReserveHub.Application.Common;
using ReserveHub.Domain.Bookings;
using ReserveHub.Domain.Common;

namespace ReserveHub.Application.Bookings;

public sealed class GetBooking(IAppDbContext db)
{
    public async Task<Result<BookingDto>> HandleAsync(Guid bookingId, CancellationToken cancellationToken)
    {
        var booking = await db.Bookings
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == bookingId, cancellationToken);

        if (booking is null)
        {
            return BookingErrors.NotFound;
        }

        return BookingDto.FromEntity(booking);
    }
}
