using ReserveHub.Domain.Bookings;

namespace ReserveHub.Application.Bookings;

public static class BookingQueries
{
    public static IQueryable<Booking> ConfirmedOverlapping(
        this IQueryable<Booking> bookings, Guid resourceId, TimeRange period)
    {
        return bookings.Where(b =>
            b.ResourceId == resourceId &&
            b.Status == BookingStatus.Confirmed &&
            b.Start < period.End &&
            period.Start < b.End);
    }
}
