using ReserveHub.Domain.Common;

namespace ReserveHub.Domain.Bookings;

public static class BookingErrors
{
    public static readonly Error InvalidPeriod = new(
        "booking.invalid_period",
        "Start must be earlier than end.",
        ErrorType.Validation);

    public static readonly Error InPast = new(
        "booking.in_past",
        "A booking cannot start in the past.",
        ErrorType.Validation);

    public static readonly Error InvalidBookedBy = new(
        "booking.invalid_booked_by",
        $"BookedBy must contain 1 to {Booking.BookedByMaxLength} characters.",
        ErrorType.Validation);

    public static readonly Error Overlap = new(
        "booking.overlap",
        "The resource is already booked for this time.",
        ErrorType.Conflict);

    public static readonly Error AlreadyCancelled = new(
        "booking.already_cancelled",
        "The booking is already cancelled.",
        ErrorType.Conflict);

    public static readonly Error NotFound = new(
        "booking.not_found",
        "Booking was not found.",
        ErrorType.NotFound);
}
