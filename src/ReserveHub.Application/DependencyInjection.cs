using Microsoft.Extensions.DependencyInjection;
using ReserveHub.Application.Bookings;
using ReserveHub.Application.Resources;

namespace ReserveHub.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<SearchResources>();
        services.AddScoped<GetAvailability>();
        services.AddScoped<CreateBooking>();
        services.AddScoped<GetBooking>();
        services.AddScoped<CancelBooking>();
        return services;
    }
}
