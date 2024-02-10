﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetSchool.Context;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NetSchool.Context.Migrations.PgSql.Migrations
{
    [DbContext(typeof(MainDbContext))]
    partial class MainDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("NetSchool.Context.Entities.Card", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("CardCollectionId")
                        .HasColumnType("integer");

                    b.Property<string>("Front")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Reverse")
                        .IsRequired()
                        .HasMaxLength(400)
                        .HasColumnType("character varying(400)");

                    b.Property<Guid>("Uid")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("CardCollectionId");

                    b.HasIndex("Uid")
                        .IsUnique();

                    b.ToTable("cards", (string)null);
                });

            modelBuilder.Entity("NetSchool.Context.Entities.CardCollection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime?>("TimeExpiration")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("Uid")
                        .HasColumnType("uuid");

                    b.Property<int?>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Uid")
                        .IsUnique();

                    b.HasIndex("UserId");

                    b.ToTable("card_collections", (string)null);
                });

            modelBuilder.Entity("NetSchool.Context.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("NickName")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("character varying(25)");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<Guid>("Uid")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("Uid")
                        .IsUnique();

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("NetSchool.Context.Entities.Card", b =>
                {
                    b.HasOne("NetSchool.Context.Entities.CardCollection", "CardCollection")
                        .WithMany("Cards")
                        .HasForeignKey("CardCollectionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("CardCollection");
                });

            modelBuilder.Entity("NetSchool.Context.Entities.CardCollection", b =>
                {
                    b.HasOne("NetSchool.Context.Entities.User", "User")
                        .WithMany("CardCollections")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.Navigation("User");
                });

            modelBuilder.Entity("NetSchool.Context.Entities.CardCollection", b =>
                {
                    b.Navigation("Cards");
                });

            modelBuilder.Entity("NetSchool.Context.Entities.User", b =>
                {
                    b.Navigation("CardCollections");
                });
#pragma warning restore 612, 618
        }
    }
}