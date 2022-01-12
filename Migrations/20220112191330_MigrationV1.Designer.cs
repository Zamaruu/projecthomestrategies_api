﻿// <auto-generated />
using System;
using HomeStrategiesApi.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace projecthomestrategies_api.Migrations
{
    [DbContext(typeof(HomeStrategiesContext))]
    [Migration("20220112191330_MigrationV1")]
    partial class MigrationV1
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.13");

            modelBuilder.Entity("HomeStrategiesApi.Models.Bill", b =>
                {
                    b.Property<int>("BillId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<float>("Amount")
                        .HasColumnType("float");

                    b.Property<int?>("BuyerUserId")
                        .HasColumnType("int");

                    b.Property<int?>("CategoryBillCategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime");

                    b.Property<int?>("HouseholdId")
                        .HasColumnType("int");

                    b.HasKey("BillId");

                    b.HasIndex("BuyerUserId");

                    b.HasIndex("CategoryBillCategoryId");

                    b.HasIndex("HouseholdId");

                    b.ToTable("Bills");
                });

            modelBuilder.Entity("HomeStrategiesApi.Models.BillCategory", b =>
                {
                    b.Property<int>("BillCategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("BillCategoryName")
                        .HasColumnType("text");

                    b.Property<int?>("HouseholdId")
                        .HasColumnType("int");

                    b.HasKey("BillCategoryId");

                    b.HasIndex("HouseholdId");

                    b.ToTable("BillCategories");
                });

            modelBuilder.Entity("HomeStrategiesApi.Models.Household", b =>
                {
                    b.Property<int>("HouseholdId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("HouseholdName")
                        .HasColumnType("text");

                    b.HasKey("HouseholdId");

                    b.ToTable("Households");
                });

            modelBuilder.Entity("HomeStrategiesApi.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("Firstname")
                        .HasColumnType("text");

                    b.Property<int?>("HouseholdId")
                        .HasColumnType("int");

                    b.Property<string>("Surname")
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<int>("UserColor")
                        .HasColumnType("int");

                    b.HasKey("UserId");

                    b.HasIndex("HouseholdId");

                    b.ToTable("User");
                });

            modelBuilder.Entity("HomeStrategiesApi.Models.Bill", b =>
                {
                    b.HasOne("HomeStrategiesApi.Models.User", "Buyer")
                        .WithMany()
                        .HasForeignKey("BuyerUserId");

                    b.HasOne("HomeStrategiesApi.Models.BillCategory", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryBillCategoryId");

                    b.HasOne("HomeStrategiesApi.Models.Household", "Household")
                        .WithMany("HouseholdBills")
                        .HasForeignKey("HouseholdId");

                    b.Navigation("Buyer");

                    b.Navigation("Category");

                    b.Navigation("Household");
                });

            modelBuilder.Entity("HomeStrategiesApi.Models.BillCategory", b =>
                {
                    b.HasOne("HomeStrategiesApi.Models.Household", "Household")
                        .WithMany()
                        .HasForeignKey("HouseholdId");

                    b.Navigation("Household");
                });

            modelBuilder.Entity("HomeStrategiesApi.Models.User", b =>
                {
                    b.HasOne("HomeStrategiesApi.Models.Household", "Household")
                        .WithMany("HouseholdMember")
                        .HasForeignKey("HouseholdId");

                    b.Navigation("Household");
                });

            modelBuilder.Entity("HomeStrategiesApi.Models.Household", b =>
                {
                    b.Navigation("HouseholdBills");

                    b.Navigation("HouseholdMember");
                });
#pragma warning restore 612, 618
        }
    }
}