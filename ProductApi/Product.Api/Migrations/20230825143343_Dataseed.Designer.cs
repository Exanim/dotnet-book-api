﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Products.Api.DbContexts;

#nullable disable

namespace Products.Api.Migrations
{
    [DbContext(typeof(ProductContext))]
    [Migration("20230825143343_Dataseed")]
    partial class Dataseed
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.0");

            modelBuilder.Entity("Products.Api.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<int>("Price")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "A fejedre megy",
                            Price = 10,
                            ProductName = "Kalap"
                        },
                        new
                        {
                            Id = 2,
                            Description = "A labadra megy",
                            Price = 15,
                            ProductName = "Cipo"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
