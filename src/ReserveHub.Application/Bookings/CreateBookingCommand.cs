namespace ReserveHub.Application.Bookings;

public sealed record CreateBookingCommand(
    Guid ResourceId,
    string BookedBy,
    DateTimeOffset Start,
    DateTimeOffset End);
