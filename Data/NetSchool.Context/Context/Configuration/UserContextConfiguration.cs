using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NetSchool.Context.Entities;

namespace NetSchool.Context.Context.Configuration;

public static class UserContextConfiguration
{
    public static void ConfigureUsers(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<IdentityRole<Guid>>().ToTable("user_roles");
        modelBuilder.Entity<IdentityUserToken<Guid>>().ToTable("user_tokens");
        modelBuilder.Entity<IdentityUserRole<Guid>>().ToTable("user_role_owners");
        modelBuilder.Entity<IdentityRoleClaim<Guid>>().ToTable("user_role_claims");
        modelBuilder.Entity<IdentityUserLogin<Guid>>().ToTable("user_logins");
        modelBuilder.Entity<IdentityUserClaim<Guid>>().ToTable("user_claims");
        modelBuilder.Entity<User>().HasMany(x => x.CardCollections).WithOne(x => x.User).OnDelete(DeleteBehavior.Cascade);
    }
}
