﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Web.Data;

#nullable disable

namespace Web.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.12");

            modelBuilder.Entity("CategoryFoodItem", b =>
                {
                    b.Property<int>("CategoriesId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("FoodItemsId")
                        .HasColumnType("INTEGER");

                    b.HasKey("CategoriesId", "FoodItemsId");

                    b.HasIndex("FoodItemsId");

                    b.ToTable("CategoryFoodItem");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Web.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Web.Models.CustomFood", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("CustomFoods");
                });

            modelBuilder.Entity("Web.Models.Donation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CustomFoodId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CustomFoodId");

                    b.HasIndex("UserId");

                    b.ToTable("Donations");
                });

            modelBuilder.Entity("Web.Models.DonationRequest", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<int?>("DonationId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("DonorId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RecipientId")
                        .HasColumnType("TEXT");

                    b.Property<int>("Status")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("DonationId");

                    b.HasIndex("DonorId");

                    b.HasIndex("RecipientId");

                    b.ToTable("DonationRequests");
                });

            modelBuilder.Entity("Web.Models.FoodItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("CarbonFootprint")
                        .HasColumnType("REAL");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("TEXT");


                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ExpiryDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("HouseholdId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ImageFilePath")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsCustom")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Status")
                        .HasColumnType("INTEGER");

                    b.Property<double>("Weight")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("HouseholdId");

                    b.ToTable("FoodItems");
                });

            modelBuilder.Entity("Web.Models.FoodItemCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("CategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("FoodItemId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("FoodItemId");

                    b.ToTable("FoodItemCategories");
                });

            modelBuilder.Entity("Web.Models.GroceryFoodItem", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<double?>("CarbonFootprint")
                        .HasColumnType("REAL");

                    b.Property<int?>("CategoryId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<string>("ExtraNote")
                        .HasColumnType("TEXT");

                    b.Property<int?>("HouseholdId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ImageFilePath")
                        .HasColumnType("TEXT");

                    b.Property<bool>("InBasket")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Quantity")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("HouseholdId");

                    b.ToTable("GroceryFood");
                });

            modelBuilder.Entity("Web.Models.Household", b =>
                {
                    b.Property<int>("HouseholdId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Address")
                        .HasColumnType("TEXT");

                    b.Property<string>("InviteLink")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("OwnerId")
                        .HasColumnType("TEXT");

                    b.HasKey("HouseholdId");

                    b.HasIndex("OwnerId")
                        .IsUnique();

                    b.ToTable("Household");
                });

            modelBuilder.Entity("Web.Models.MessageHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DonationRequestId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DonationRequestId");

                    b.HasIndex("UserId");

                    b.ToTable("ChatMessages");
                });

            modelBuilder.Entity("Web.Models.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Date")
                        .HasColumnType("TEXT");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("Read")
                        .HasColumnType("INTEGER");

                    b.Property<string>("System")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("Web.Models.Recipe", b =>
                {
                    b.Property<string>("recipeName")
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<decimal>("difficulty")
                        .HasColumnType("decimal(2,1)");

                    b.Property<int>("duration")
                        .HasColumnType("INTEGER");

                    b.Property<string>("imageFilePath")
                        .HasColumnType("TEXT");

                    b.Property<string>("ingredientAmount")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("ingredients")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("reviews")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("recipeName");

                    b.ToTable("Recipes");
                });

            modelBuilder.Entity("Web.Models.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Disabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("HouseholdId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ImageURL")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("HouseholdId");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("CategoryFoodItem", b =>
                {
                    b.HasOne("Web.Models.Category", null)
                        .WithMany()
                        .HasForeignKey("CategoriesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Web.Models.FoodItem", null)
                        .WithMany()
                        .HasForeignKey("FoodItemsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Web.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Web.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Web.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Web.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Web.Models.Donation", b =>
                {
                    b.HasOne("Web.Models.CustomFood", "CustomFood")
                        .WithMany()
                        .HasForeignKey("CustomFoodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Web.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("CustomFood");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Web.Models.DonationRequest", b =>
                {
                    b.HasOne("Web.Models.Donation", "Donation")
                        .WithMany()
                        .HasForeignKey("DonationId");

                    b.HasOne("Web.Models.User", "Donor")
                        .WithMany()
                        .HasForeignKey("DonorId");

                    b.HasOne("Web.Models.User", "Recipient")
                        .WithMany()
                        .HasForeignKey("RecipientId");

                    b.Navigation("Donation");

                    b.Navigation("Donor");

                    b.Navigation("Recipient");
                });

            modelBuilder.Entity("Web.Models.FoodItem", b =>
                {
                    b.HasOne("Web.Models.Household", "Household")
                        .WithMany()
                        .HasForeignKey("HouseholdId");

                    b.Navigation("Household");
                });

            modelBuilder.Entity("Web.Models.FoodItemCategory", b =>
                {
                    b.HasOne("Web.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId");

                    b.HasOne("Web.Models.FoodItem", "FoodItem")
                        .WithMany()
                        .HasForeignKey("FoodItemId");

                    b.Navigation("Category");

                    b.Navigation("FoodItem");
                });

            modelBuilder.Entity("Web.Models.GroceryFoodItem", b =>
                {
                    b.HasOne("Web.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId");

                    b.HasOne("Web.Models.Household", "Household")
                        .WithMany("GroceryItems")
                        .HasForeignKey("HouseholdId");

                    b.Navigation("Category");

                    b.Navigation("Household");
                });

            modelBuilder.Entity("Web.Models.Household", b =>
                {
                    b.HasOne("Web.Models.User", "Owner")
                        .WithOne("OwnerOf")
                        .HasForeignKey("Web.Models.Household", "OwnerId");

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("Web.Models.MessageHistory", b =>
                {
                    b.HasOne("Web.Models.DonationRequest", "DonationRequest")
                        .WithMany()
                        .HasForeignKey("DonationRequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Web.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("DonationRequest");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Web.Models.Notification", b =>
                {
                    b.HasOne("Web.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("Web.Models.User", b =>
                {
                    b.HasOne("Web.Models.Household", "Household")
                        .WithMany("Users")
                        .HasForeignKey("HouseholdId");

                    b.Navigation("Household");
                });

            modelBuilder.Entity("Web.Models.Household", b =>
                {
                    b.Navigation("GroceryItems");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("Web.Models.User", b =>
                {
                    b.Navigation("OwnerOf");
                });
#pragma warning restore 612, 618
        }
    }
}
