using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Openbook.Identity;

public class ApplicationIdentityDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options)
        : base(options) { }

    // Parameterless constructor for design-time support
    public ApplicationIdentityDbContext() : base() { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<ApplicationUser>()
            .Property(u => u.Name)
            .IsRequired()
            .HasMaxLength(255);

        // Seed admin role
        var adminRoleId = "b1e1e1e1-e1e1-e1e1-e1e1-b1e1e1e1e1e1";
        var adminUserId = "a1a1a1a1-a1a1-a1a1-a1a1-a1a1a1a1a1a1";
        var adminRole = new IdentityRole
        {
            Id = adminRoleId,
            Name = "Admin",
            NormalizedName = "ADMIN"
        };
        var adminUser = new ApplicationUser
        {
            Id = adminUserId,
            UserName = "admin",
            NormalizedUserName = "ADMIN",
            Email = "admin@openbook.local",
            NormalizedEmail = "ADMIN@OPENBOOK.LOCAL",
            EmailConfirmed = true,
            Name = "Admin",
            PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(null!, "qaz123WSX")
        };
        modelBuilder.Entity<IdentityRole>().HasData(adminRole);
        modelBuilder.Entity<ApplicationUser>().HasData(adminUser);
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                UserId = adminUserId,
                RoleId = adminRoleId
            });
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=identitydb;Username=appuser;Password=apppassword");
        }
    }
}

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; } = string.Empty;
}
