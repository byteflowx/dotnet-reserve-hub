using ReserveHub.Domain.Common;

namespace ReserveHub.Domain.Bookings;

public sealed class Booking
{
    public const int BookedByMaxLength = 100;

    private Booking()
    {
    }

    private Booking(Guid resourceId, string bookedBy, TimeRange period, DateTimeOffset createdAt)
    {
        Id = Guid.NewGuid();
        ResourceId = resourceId;
        BookedBy = bookedBy;
        Start = period.Start;
        End = period.End;
        Status = BookingStatus.Confirmed;
        CreatedAt = createdAt;
    }

    public Guid Id { get; private set; }

    public Guid ResourceId { get; private set; }

    public string BookedBy { get; private set; } = string.Empty;

    public DateTimeOffset Start { get; private set; }

    public DateTimeOffset End { get; private set; }

    public BookingStatus Status { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public static Result<Booking> Create(Guid resourceId, string bookedBy, TimeRange period, DateTimeOffset now)
    {
        if (string.IsNullOrWhiteSpace(bookedBy))
        {
            return BookingErrors.InvalidBookedBy;
        }

        var trimmedBookedBy = bookedBy.Trim();
        if (trimmedBookedBy.Length > BookedByMaxLength)
        {
            return BookingErrors.InvalidBookedBy;
        }

        if (period.Start < now)
        {
            return BookingErrors.InPast;
        }

        return new Booking(resourceId, trimmedBookedBy, period, now);
    }

    public Result Cancel()
    {
        if (Status == BookingStatus.Cancelled)
        {
            return BookingErrors.AlreadyCancelled;
        }

        Status = BookingStatus.Cancelled;
        return Result.Success();
    }
}
