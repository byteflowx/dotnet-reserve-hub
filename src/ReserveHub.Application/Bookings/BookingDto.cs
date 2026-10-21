using ReserveHub.Domain.Bookings;

namespace ReserveHub.Application.Bookings;

public sealed record BookingDto(
    Guid Id,
    Guid ResourceId,
    string BookedBy,
    DateTimeOffset Start,
    DateTimeOffset End,
    BookingStatus Status,
    DateTimeOffset CreatedAt)
{
    public static BookingDto FromEntity(Booking booking) => new(
        booking.Id,
        booking.ResourceId,
        booking.BookedBy,
        booking.Start,
        booking.End,
        booking.Status,
        booking.CreatedAt);
}
