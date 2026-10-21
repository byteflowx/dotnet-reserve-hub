using System.ComponentModel;
using ModelContextProtocol;
using ModelContextProtocol.Server;
using ReserveHub.Application.Bookings;
using ReserveHub.Application.Resources;
using ReserveHub.Domain.Common;
using ReserveHub.Domain.Resources;

namespace ReserveHub.Api.Mcp;

[McpServerToolType]
public sealed class ReserveHubTools(
    SearchResources searchResources,
    GetAvailability getAvailability,
    GetBooking getBooking)
{
    [McpServerTool(Name = "search_resources", ReadOnly = true)]
    [Description("Searches bookable resources (rooms, desks, equipment). Returns id, name, type and capacity of each one.")]
    public async Task<IReadOnlyList<ResourceDto>> SearchResources(
        [Description("Resource type: Room, Desk or Equipment. Omit to search all types.")] ResourceType? type = null,
        [Description("Minimum capacity (number of people). Omit to accept any capacity.")] int? minCapacity = null,
        CancellationToken cancellationToken = default)
    {
        return await searchResources.HandleAsync(type, minCapacity, cancellationToken);
    }

    [McpServerTool(Name = "get_availability", ReadOnly = true)]
    [Description("Checks whether a resource is free in the time range [start, end) and lists the confirmed bookings that block it.")]
    public async Task<AvailabilityDto> GetAvailability(
        [Description("Resource id, as returned by search_resources.")] Guid resourceId,
        [Description("Start of the range, ISO 8601 with offset, e.g. 2030-01-01T14:00:00Z.")] DateTimeOffset start,
        [Description("End of the range (exclusive), ISO 8601 with offset, e.g. 2030-01-01T15:00:00Z.")] DateTimeOffset end,
        CancellationToken cancellationToken = default)
    {
        var result = await getAvailability.HandleAsync(resourceId, start, end, cancellationToken);
        return ValueOrThrow(result);
    }

    [McpServerTool(Name = "get_booking", ReadOnly = true)]
    [Description("Returns a booking by its id: resource, who booked it, time range and status.")]
    public async Task<BookingDto> GetBooking(
        [Description("Booking id.")] Guid bookingId,
        CancellationToken cancellationToken = default)
    {
        var result = await getBooking.HandleAsync(bookingId, cancellationToken);
        return ValueOrThrow(result);
    }

    private static T ValueOrThrow<T>(Result<T> result)
    {
        if (result.IsFailure)
        {
            throw new McpException($"{result.Error.Code}: {result.Error.Message}");
        }

        return result.Value;
    }
}
