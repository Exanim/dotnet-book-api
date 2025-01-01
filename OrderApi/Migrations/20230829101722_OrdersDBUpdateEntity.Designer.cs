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
    [Migration("20230829101722_OrdersDBUpdateEntity")]
    partial class OrdersDBUpdateEntity
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

                    b.HasIndex("UserId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Org.OpenAPITools.Models.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("OrderEntityOrderId")
                        .HasColumnType("INTEGER");

                    b.HasKey("ProductId");

                    b.HasIndex("OrderEntityOrderId");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("Org.OpenAPITools.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("UserId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("OrderApi.Entities.OrderEntity", b =>
                {
                    b.HasOne("Org.OpenAPITools.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Org.OpenAPITools.Models.Product", b =>
                {
                    b.HasOne("OrderApi.Entities.OrderEntity", null)
                        .WithMany("Products")
                        .HasForeignKey("OrderEntityOrderId");
                });

            modelBuilder.Entity("OrderApi.Entities.OrderEntity", b =>
                {
                    b.Navigation("Products");
                });
#pragma warning restore 612, 618
        }
    }
}
