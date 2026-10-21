using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ReserveHub.Domain.Resources;

namespace ReserveHub.Infrastructure.Persistence.Configurations;

internal sealed class ResourceConfiguration : IEntityTypeConfiguration<Resource>
{
    public void Configure(EntityTypeBuilder<Resource> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.Name)
            .HasMaxLength(Resource.NameMaxLength)
            .IsRequired();

        builder.Property(r => r.Type)
            .HasConversion<string>()
            .HasMaxLength(20);

        builder.HasData(
            new Resource(Guid.Parse("11111111-1111-1111-1111-111111111111"), "Room Alpha", ResourceType.Room, 6),
            new Resource(Guid.Parse("22222222-2222-2222-2222-222222222222"), "Room Beta", ResourceType.Room, 12),
            new Resource(Guid.Parse("33333333-3333-3333-3333-333333333333"), "Desk 12", ResourceType.Desk, 1),
            new Resource(Guid.Parse("44444444-4444-4444-4444-444444444444"), "Desk 14", ResourceType.Desk, 1),
            new Resource(Guid.Parse("55555555-5555-5555-5555-555555555555"), "Projector P1", ResourceType.Equipment, 1));
    }
}
