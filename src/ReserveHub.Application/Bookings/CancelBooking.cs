using Microsoft.EntityFrameworkCore;
using ReserveHub.Application.Common;
using ReserveHub.Domain.Bookings;
using ReserveHub.Domain.Common;

namespace ReserveHub.Application.Bookings;

public sealed class CancelBooking(IAppDbContext db)
{
    public async Task<Result<BookingDto>> HandleAsync(Guid bookingId, CancellationToken cancellationToken)
    {
        var booking = await db.Bookings.FirstOrDefaultAsync(b => b.Id == bookingId, cancellationToken);
        if (booking is null)
        {
            return BookingErrors.NotFound;
        }

        var cancelResult = booking.Cancel();
        if (cancelResult.IsFailure)
        {
            return cancelResult.Error;
        }

        await db.SaveChangesAsync(cancellationToken);
        return BookingDto.FromEntity(booking);
    }
}
