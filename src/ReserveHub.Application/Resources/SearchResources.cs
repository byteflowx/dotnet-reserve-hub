using Microsoft.EntityFrameworkCore;
using ReserveHub.Application.Common;
using ReserveHub.Domain.Resources;

namespace ReserveHub.Application.Resources;

public sealed class SearchResources(IAppDbContext db)
{
    public async Task<IReadOnlyList<ResourceDto>> HandleAsync(
        ResourceType? type, int? minCapacity, CancellationToken cancellationToken)
    {
        var query = db.Resources.AsNoTracking();

        if (type is not null)
        {
            query = query.Where(r => r.Type == type.Value);
        }

        if (minCapacity is not null)
        {
            query = query.Where(r => r.Capacity >= minCapacity.Value);
        }

        return await query
            .OrderBy(r => r.Name)
            .Select(r => new ResourceDto(r.Id, r.Name, r.Type, r.Capacity))
            .ToListAsync(cancellationToken);
    }
}
