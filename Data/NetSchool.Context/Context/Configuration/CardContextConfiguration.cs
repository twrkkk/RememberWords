using Microsoft.EntityFrameworkCore;
using NetSchool.Context.Entities;

namespace NetSchool.Context.Context.Configuration;

public static class CardContextConfiguration
{
    public static void ConfigureCards(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Card>().ToTable("cards");
        modelBuilder.Entity<Card>().Property(x => x.Front).IsRequired();
        modelBuilder.Entity<Card>().Property(x => x.Front).HasMaxLength(200);
        modelBuilder.Entity<Card>().Property(x => x.Reverse).IsRequired();
        modelBuilder.Entity<Card>().Property(x => x.Reverse).HasMaxLength(400);
    }
}
