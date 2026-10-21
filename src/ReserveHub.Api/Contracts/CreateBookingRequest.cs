using ReserveHub.Application.Bookings;

namespace ReserveHub.Api.Contracts;

public sealed record CreateBookingRequest
{
    public required Guid ResourceId { get; init; }

    public required string BookedBy { get; init; }

    public required DateTimeOffset Start { get; init; }

    public required DateTimeOffset End { get; init; }

    public CreateBookingCommand ToCommand() => new(ResourceId, BookedBy, Start, End);
}
