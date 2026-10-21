namespace ReserveHub.Application.Resources;

public sealed record AvailabilityDto(
    Guid ResourceId,
    DateTimeOffset Start,
    DateTimeOffset End,
    bool IsAvailable,
    IReadOnlyList<TimeSlotDto> BusySlots);
