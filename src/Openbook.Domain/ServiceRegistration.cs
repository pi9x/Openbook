using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Openbook.Domain;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationDbContext(this IServiceCollection services, string connectionString)
    {
        services.AddPooledDbContextFactory<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString));

        return services;
    }
}