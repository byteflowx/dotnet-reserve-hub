namespace ReserveHub.Application.Bookings;

public sealed class BookingOverlapException(Exception innerException)
    : Exception("The booking overlaps another confirmed booking.", innerException);
