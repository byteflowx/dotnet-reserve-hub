using System.Net;
using System.Net.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReserveHub.Infrastructure.Persistence;

namespace ReserveHub.IntegrationTests;

public class ConcurrencyTests(ReserveHubApiFactory factory) : IClassFixture<ReserveHubApiFactory>
{
    [Fact]
    public async Task Parallel_requests_for_the_same_slot_create_exactly_one_booking()
    {
        const int requestCount = 10;
        var client = factory.CreateClient();
        var start = DateTimeOffset.UtcNow.AddDays(1);
        var body = new
        {
            resourceId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            bookedBy = "Anna",
            start,
            end = start.AddHours(1)
        };

        var requests = Enumerable.Range(0, requestCount)
            .Select(_ => client.PostAsJsonAsync("/api/bookings", body));
        var responses = await Task.WhenAll(requests);

        Assert.Equal(1, responses.Count(r => r.StatusCode == HttpStatusCode.Created));
        Assert.Equal(requestCount - 1, responses.Count(r => r.StatusCode == HttpStatusCode.Conflict));

        using var scope = factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        Assert.Equal(1, await db.Bookings.CountAsync());
    }
}
