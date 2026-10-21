using Microsoft.EntityFrameworkCore;
using ReserveHub.Application.Common;
using ReserveHub.Domain.Bookings;
using ReserveHub.Domain.Common;
using ReserveHub.Domain.Resources;

namespace ReserveHub.Application.Bookings;

public sealed class CreateBooking(IAppDbContext db, TimeProvider timeProvider)
{
    public async Task<Result<BookingDto>> HandleAsync(CreateBookingCommand command, CancellationToken cancellationToken)
    {
        var periodResult = TimeRange.Create(command.Start, command.End);
        if (periodResult.IsFailure)
        {
            return periodResult.Error;
        }

        var period = periodResult.Value;

        var resourceExists = await db.Resources.AnyAsync(r => r.Id == command.ResourceId, cancellationToken);
        if (!resourceExists)
        {
            return ResourceErrors.NotFound;
        }

        var bookingResult = Booking.Create(command.ResourceId, command.BookedBy, period, timeProvider.GetUtcNow());
        if (bookingResult.IsFailure)
        {
            return bookingResult.Error;
        }

        var booking = bookingResult.Value;

        var hasOverlap = await db.Bookings
            .ConfirmedOverlapping(command.ResourceId, period)
            .AnyAsync(cancellationToken);
        if (hasOverlap)
        {
            return BookingErrors.Overlap;
        }

        db.Bookings.Add(booking);
        try
        {
            await db.SaveChangesAsync(cancellationToken);
        }
        catch (BookingOverlapException)
        {
            return BookingErrors.Overlap;
        }

        return BookingDto.FromEntity(booking);
    }
}
