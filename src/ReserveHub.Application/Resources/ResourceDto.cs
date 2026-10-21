using ReserveHub.Domain.Resources;

namespace ReserveHub.Application.Resources;

public sealed record ResourceDto(Guid Id, string Name, ResourceType Type, int Capacity);
