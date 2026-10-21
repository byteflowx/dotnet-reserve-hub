using Microsoft.AspNetCore.Mvc;
using ReserveHub.Api.Contracts;
using ReserveHub.Application.Bookings;

namespace ReserveHub.Api.Controllers;

[ApiController]
[Route("api/bookings")]
public sealed class BookingsController(
    CreateBooking createBooking,
    GetBooking getBooking,
    CancelBooking cancelBooking) : ApiControllerBase
{
    [HttpPost]
    [ProducesResponseType<BookingDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<BookingDto>> Create(
        CreateBookingRequest request,
        CancellationToken cancellationToken)
    {
        var result = await createBooking.HandleAsync(request.ToCommand(), cancellationToken);
        if (result.IsFailure)
        {
            return ErrorResponse(result.Error);
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType<BookingDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<BookingDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await getBooking.HandleAsync(id, cancellationToken);
        if (result.IsFailure)
        {
            return ErrorResponse(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpPost("{id:guid}/cancel")]
    [ProducesResponseType<BookingDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<BookingDto>> Cancel(Guid id, CancellationToken cancellationToken)
    {
        var result = await cancelBooking.HandleAsync(id, cancellationToken);
        if (result.IsFailure)
        {
            return ErrorResponse(result.Error);
        }

        return Ok(result.Value);
    }
}
