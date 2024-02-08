using Microsoft.EntityFrameworkCore;
using NetSchool.Context.Entities;

namespace NetSchool.Context.Context.Configuration;

public static class CardCollectionContextConfiguration
{
    public static void ConfigureCardCollections(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CardCollection>().ToTable("card_collections");
        modelBuilder.Entity<CardCollection>().Property(x => x.Name).IsRequired();
        modelBuilder.Entity<CardCollection>().Property(x => x.Name).HasMaxLength(100);
        modelBuilder.Entity<CardCollection>().HasMany(x => x.Cards).WithOne(x=>x.CardCollection).OnDelete(DeleteBehavior.Cascade);
    }
}
