using Microsoft.EntityFrameworkCore;
using NetSchool.Context.Entities;

namespace NetSchool.Context.Context.Configuration;

public static class UserContextConfiguration
{
    public static void ConfigureUsers(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("users");
        modelBuilder.Entity<User>().Property(x => x.NickName).IsRequired();
        modelBuilder.Entity<User>().Property(x => x.NickName).HasMaxLength(25);
        modelBuilder.Entity<User>().HasMany(x => x.CardCollections).WithOne(x => x.User).OnDelete(DeleteBehavior.Cascade);
    }
}
