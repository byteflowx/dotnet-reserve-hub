using ReserveHub.Domain.Common;

namespace ReserveHub.Domain.Bookings;

public readonly record struct TimeRange
{
    private TimeRange(DateTimeOffset start, DateTimeOffset end)
    {
        Start = start;
        End = end;
    }

    public DateTimeOffset Start { get; }

    public DateTimeOffset End { get; }

    public static Result<TimeRange> Create(DateTimeOffset start, DateTimeOffset end)
    {
        if (start >= end)
        {
            return BookingErrors.InvalidPeriod;
        }

        return new TimeRange(start.ToUniversalTime(), end.ToUniversalTime());
    }

    public bool Overlaps(TimeRange other) => Start < other.End && other.Start < End;
}
