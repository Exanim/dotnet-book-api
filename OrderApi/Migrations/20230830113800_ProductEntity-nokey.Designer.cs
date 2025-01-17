﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using OrderApi.DbContexts;

#nullable disable

namespace OrderApi.Migrations
{
    [DbContext(typeof(OrdersContext))]
    [Migration("20230830113800_ProductEntity-nokey")]
    partial class ProductEntitynokey
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.0");

            modelBuilder.Entity("OrderApi.Entities.OrderEntity", b =>
                {
                    b.Property<int>("OrderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("UserId")
                        .HasColumnType("INTEGER");

                    b.HasKey("OrderId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("OrderApi.Entities.ProductEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("OrderEntityOrderId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");
                    
                    b.HasIndex("OrderEntityOrderId");

                    b.ToTable("ProductEntity");
                });

            modelBuilder.Entity("OrderApi.Entities.ProductEntity", b =>
                {
                    b.HasOne("OrderApi.Entities.OrderEntity", null)
                        .WithMany("ProductIds")
                        .HasForeignKey("OrderEntityOrderId");
                });

            modelBuilder.Entity("OrderApi.Entities.OrderEntity", b =>
                {
                    b.Navigation("ProductIds");
                });
#pragma warning restore 612, 618
        }
    }
}
