﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NetSchool.Context;

#nullable disable

namespace NetSchool.Context.Migrations.MSSQL.Migrations
{
    [DbContext(typeof(MainDbContext))]
    [Migration("20240208130455_init")]
    partial class init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("NetSchool.Context.Entities.Card", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("CardCollectionId")
                        .HasColumnType("int");

                    b.Property<string>("Front")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Reverse")
                        .IsRequired()
                        .HasMaxLength(400)
                        .HasColumnType("nvarchar(400)");

                    b.Property<Guid>("Uid")
                        .HasColumnType("uniqueidentifier");

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
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime?>("TimeExpiration")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("Uid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

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
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("NickName")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<Guid>("Uid")
                        .HasColumnType("uniqueidentifier");

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
