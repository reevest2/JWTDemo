using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace JWTDemo;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Set the schema for the ASP.NET Identity model
        builder.HasDefaultSchema("Identity");

        // Alternative: Explicitly setting schemas for each table
        builder.Entity<IdentityUser>().ToTable("AspNetUsers", "Identity");
        builder.Entity<IdentityRole>().ToTable("AspNetRoles", "Identity");
        builder.Entity<IdentityUserRole<string>>().ToTable("AspNetUserRoles", "Identity");
        builder.Entity<IdentityUserClaim<string>>().ToTable("AspNetUserClaims", "Identity");
        builder.Entity<IdentityUserLogin<string>>().ToTable("AspNetUserLogins", "Identity");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("AspNetRoleClaims", "Identity");
        builder.Entity<IdentityUserToken<string>>().ToTable("AspNetUserTokens", "Identity");
    }
}