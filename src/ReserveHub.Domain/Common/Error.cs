namespace ReserveHub.Domain.Common;

public sealed record Error(string Code, string Message, ErrorType Type);
