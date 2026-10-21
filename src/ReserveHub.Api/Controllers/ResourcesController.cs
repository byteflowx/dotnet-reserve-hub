using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ReserveHub.Application.Resources;
using ReserveHub.Domain.Resources;

namespace ReserveHub.Api.Controllers;

[ApiController]
[Route("api/resources")]
public sealed class ResourcesController(
    SearchResources searchResources,
    GetAvailability getAvailability) : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType<IReadOnlyList<ResourceDto>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ResourceDto>>> Search(
        [FromQuery] ResourceType? type,
        [FromQuery] int? minCapacity,
        CancellationToken cancellationToken)
    {
        var resources = await searchResources.HandleAsync(type, minCapacity, cancellationToken);
        return Ok(resources);
    }

    [HttpGet("{id:guid}/availability")]
    [ProducesResponseType<AvailabilityDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AvailabilityDto>> GetAvailability(
        Guid id,
        [FromQuery, BindRequired] DateTimeOffset start,
        [FromQuery, BindRequired] DateTimeOffset end,
        CancellationToken cancellationToken)
    {
        var result = await getAvailability.HandleAsync(id, start, end, cancellationToken);
        if (result.IsFailure)
        {
            return ErrorResponse(result.Error);
        }

        return Ok(result.Value);
    }
}
