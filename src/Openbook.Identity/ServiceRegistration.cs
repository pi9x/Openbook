using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Openbook.Identity;

public static class ServiceRegistration
{
    public static IServiceCollection AddApplicationIdentityDbContext(this IServiceCollection services)
    {
        services.AddDbContext<ApplicationIdentityDbContext>(options =>
            options.UseNpgsql("Host=localhost;Port=5432;Database=identitydb;Username=appuser;Password=apppassword"));

        services.AddScoped<PasswordHasher<ApplicationUser>>();
        services.AddScoped<UserStore<ApplicationUser, IdentityRole, ApplicationIdentityDbContext>>();
        services.AddScoped<RoleStore<IdentityRole, ApplicationIdentityDbContext>>();

        return services;
    }
}