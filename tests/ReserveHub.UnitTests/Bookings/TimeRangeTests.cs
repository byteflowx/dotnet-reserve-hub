using ReserveHub.Domain.Bookings;

namespace ReserveHub.UnitTests.Bookings;

public class TimeRangeTests
{
    private static readonly DateTimeOffset Day = new(2030, 1, 1, 0, 0, 0, TimeSpan.Zero);

    [Theory]
    [InlineData(11, 0, 13, 0, true)]
    [InlineData(12, 0, 13, 0, false)]
    [InlineData(9, 0, 10, 0, false)]
    [InlineData(10, 30, 11, 0, true)]
    [InlineData(9, 0, 13, 0, true)]
    [InlineData(10, 0, 12, 0, true)]
    public void Overlaps_uses_half_open_intervals(
        int startHour, int startMinute, int endHour, int endMinute, bool expected)
    {
        var existing = Range(10, 0, 12, 0);
        var candidate = Range(startHour, startMinute, endHour, endMinute);

        Assert.Equal(expected, existing.Overlaps(candidate));
        Assert.Equal(expected, candidate.Overlaps(existing));
    }

    private static TimeRange Range(int startHour, int startMinute, int endHour, int endMinute)
    {
        var start = Day.AddHours(startHour).AddMinutes(startMinute);
        var end = Day.AddHours(endHour).AddMinutes(endMinute);
        return TimeRange.Create(start, end).Value;
    }
}
