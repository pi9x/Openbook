using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Openbook.Identity;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationIdentityDbContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<ApplicationIdentityDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<PasswordHasher<ApplicationUser>>();
        services.AddScoped<UserStore<ApplicationUser, IdentityRole, ApplicationIdentityDbContext>>();
        services.AddScoped<RoleStore<IdentityRole, ApplicationIdentityDbContext>>();

        return services;
    }
}