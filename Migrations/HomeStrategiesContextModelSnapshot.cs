﻿// <auto-generated />
using System;
using HomeStrategiesApi.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace projecthomestrategies_api.Migrations
{
    [DbContext(typeof(HomeStrategiesContext))]
    partial class HomeStrategiesContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime");

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

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime");

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

                    b.Property<int>("AdminId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime");

                    b.Property<string>("HouseholdName")
                        .HasColumnType("text");

                    b.HasKey("HouseholdId");

                    b.HasIndex("AdminId")
                        .IsUnique();

                    b.ToTable("Households");
                });

            modelBuilder.Entity("HomeStrategiesApi.Models.Notification", b =>
                {
                    b.Property<int>("NotificationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime");

                    b.Property<string>("CreatorName")
                        .HasColumnType("text");

                    b.Property<bool>("Seen")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.Property<int?>("UserId")
                        .HasColumnType("int");

                    b.HasKey("NotificationId");

                    b.HasIndex("UserId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("HomeStrategiesApi.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FcmToken")
                        .HasColumnType("text");

                    b.Property<string>("Firstname")
                        .HasColumnType("text");

                    b.Property<int?>("HouseholdId")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .HasColumnType("text");

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
                        .WithMany("HouseholdBillCategories")
                        .HasForeignKey("HouseholdId");

                    b.Navigation("Household");
                });

            modelBuilder.Entity("HomeStrategiesApi.Models.Household", b =>
                {
                    b.HasOne("HomeStrategiesApi.Models.User", "HouseholdCreator")
                        .WithOne("AdminOfHousehold")
                        .HasForeignKey("HomeStrategiesApi.Models.Household", "AdminId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("HouseholdCreator");
                });

            modelBuilder.Entity("HomeStrategiesApi.Models.Notification", b =>
                {
                    b.HasOne("HomeStrategiesApi.Models.User", "User")
                        .WithMany("Notifications")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
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
                    b.Navigation("HouseholdBillCategories");

                    b.Navigation("HouseholdBills");

                    b.Navigation("HouseholdMember");
                });

            modelBuilder.Entity("HomeStrategiesApi.Models.User", b =>
                {
                    b.Navigation("AdminOfHousehold");

                    b.Navigation("Notifications");
                });
#pragma warning restore 612, 618
        }
    }
}
