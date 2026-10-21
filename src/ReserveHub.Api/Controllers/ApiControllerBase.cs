using Microsoft.AspNetCore.Mvc;
using ReserveHub.Domain.Common;

namespace ReserveHub.Api.Controllers;

public abstract class ApiControllerBase : ControllerBase
{
    protected ObjectResult ErrorResponse(Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        return Problem(statusCode: statusCode, title: error.Code, detail: error.Message);
    }
}
