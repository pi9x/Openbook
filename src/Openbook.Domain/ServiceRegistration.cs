using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Openbook.Domain;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationDbContext(this IServiceCollection services)
    {
        services.AddPooledDbContextFactory<ApplicationDbContext>(options =>
            options.UseNpgsql("Host=localhost;Port=5432;Database=appdb;Username=appuser;Password=apppassword"));

        return services;
    }
}