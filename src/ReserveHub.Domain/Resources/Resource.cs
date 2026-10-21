namespace ReserveHub.Domain.Resources;

public sealed class Resource
{
    public const int NameMaxLength = 100;

    private Resource()
    {
    }

    public Resource(Guid id, string name, ResourceType type, int capacity)
    {
        Id = id;
        Name = name;
        Type = type;
        Capacity = capacity;
    }

    public Guid Id { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public ResourceType Type { get; private set; }

    public int Capacity { get; private set; }
}
