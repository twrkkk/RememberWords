namespace NetSchool.Context;

//using NetSchool.Context.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using NetSchool.Context.Entities;
using NetSchool.Context.Context.Configuration;

public class MainDbContext : DbContext
{
    public DbSet<CardCollection> CardCollections { get; set; }

    public MainDbContext(DbContextOptions<MainDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ConfigureUsers();
        modelBuilder.ConfigureCards();
        modelBuilder.ConfigureCardCollections();
    }
}
