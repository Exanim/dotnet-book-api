﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RatingApi.DbContexts;

#nullable disable

namespace RatingApi.Migrations
{
    [DbContext(typeof(RatingContext))]
    partial class RatingContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.11");

            modelBuilder.Entity("RatingApi.Entities.Rating", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ProductId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RatingValue")
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Ratings");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ProductId = 3213,
                            RatingValue = 5,
                            UserId = 321
                        },
                        new
                        {
                            Id = 2,
                            ProductId = 2001,
                            RatingValue = 4,
                            UserId = 321
                        },
                        new
                        {
                            Id = 3,
                            ProductId = 512,
                            RatingValue = 3,
                            UserId = 4
                        },
                        new
                        {
                            Id = 4,
                            ProductId = 92,
                            RatingValue = 4,
                            UserId = 51
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
