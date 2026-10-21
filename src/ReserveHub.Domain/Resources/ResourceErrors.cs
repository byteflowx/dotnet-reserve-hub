using ReserveHub.Domain.Common;

namespace ReserveHub.Domain.Resources;

public static class ResourceErrors
{
    public static readonly Error NotFound = new(
        "resource.not_found",
        "Resource was not found.",
        ErrorType.NotFound);
}
