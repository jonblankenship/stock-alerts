﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StockAlerts.Data;

namespace StockAlerts.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20190604015522_Update018")]
    partial class Update018
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("StockAlerts.Data.Model.AlertCriteria", b =>
                {
                    b.Property<Guid>("AlertCriteriaId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("AlertDefinitionId");

                    b.Property<DateTimeOffset>("Created");

                    b.Property<decimal?>("Level");

                    b.Property<DateTimeOffset>("Modified");

                    b.Property<int>("Operator");

                    b.Property<Guid?>("ParentCriteriaId");

                    b.Property<int>("Type");

                    b.HasKey("AlertCriteriaId");

                    b.HasIndex("AlertDefinitionId")
                        .IsUnique()
                        .HasFilter("[AlertDefinitionId] IS NOT NULL");

                    b.HasIndex("ParentCriteriaId");

                    b.ToTable("AlertCriteria");
                });

            modelBuilder.Entity("StockAlerts.Data.Model.AlertDefinition", b =>
                {
                    b.Property<Guid>("AlertDefinitionId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("AppUserId");

                    b.Property<DateTimeOffset>("Created");

                    b.Property<DateTimeOffset?>("LastSent");

                    b.Property<DateTimeOffset>("Modified");

                    b.Property<string>("Name");

                    b.Property<int>("Status");

                    b.Property<Guid>("StockId");

                    b.HasKey("AlertDefinitionId");

                    b.HasIndex("AppUserId");

                    b.HasIndex("StockId");

                    b.ToTable("AlertDefinitions");
                });

            modelBuilder.Entity("StockAlerts.Data.Model.AlertTriggerHistory", b =>
                {
                    b.Property<Guid>("AlertTriggerHistoryId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("AlertDefinitionId");

                    b.Property<DateTimeOffset>("Created");

                    b.Property<DateTimeOffset>("Modified");

                    b.Property<DateTimeOffset>("TimeTriggered");

                    b.HasKey("AlertTriggerHistoryId");

                    b.HasIndex("AlertDefinitionId");

                    b.ToTable("AlertTriggerHistory");
                });

            modelBuilder.Entity("StockAlerts.Data.Model.ApiCall", b =>
                {
                    b.Property<Guid>("ApiCallId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Api");

                    b.Property<DateTimeOffset>("CallTime");

                    b.Property<DateTimeOffset>("Created");

                    b.Property<DateTimeOffset>("Modified");

                    b.Property<int>("ResponseCode");

                    b.Property<string>("Route");

                    b.HasKey("ApiCallId");

                    b.ToTable("ApiCalls");
                });

            modelBuilder.Entity("StockAlerts.Data.Model.AppUser", b =>
                {
                    b.Property<Guid>("AppUserId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("Created");

                    b.Property<bool>("HasBeenGrantedAccess");

                    b.Property<DateTimeOffset>("Modified");

                    b.Property<Guid>("UserId");

                    b.Property<string>("UserName");

                    b.HasKey("AppUserId");

                    b.ToTable("AppUsers");
                });

            modelBuilder.Entity("StockAlerts.Data.Model.RefreshToken", b =>
                {
                    b.Property<Guid>("RefreshTokenId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("AppUserId");

                    b.Property<DateTimeOffset>("Created");

                    b.Property<DateTime>("Expires");

                    b.Property<DateTimeOffset>("Modified");

                    b.Property<string>("RemoteIpAddress");

                    b.Property<string>("Token");

                    b.HasKey("RefreshTokenId");

                    b.HasIndex("AppUserId");

                    b.ToTable("RefreshToken");
                });

            modelBuilder.Entity("StockAlerts.Data.Model.Stock", b =>
                {
                    b.Property<Guid>("StockId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("Created");

                    b.Property<decimal>("LastPrice");

                    b.Property<DateTimeOffset>("LastTime");

                    b.Property<DateTimeOffset>("Modified");

                    b.Property<decimal>("PreviousLastPrice");

                    b.Property<string>("Symbol");

                    b.HasKey("StockId");

                    b.ToTable("Stocks");
                });

            modelBuilder.Entity("StockAlerts.Data.Model.UserPreferences", b =>
                {
                    b.Property<Guid>("UserPreferencesId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("AppUserId");

                    b.Property<DateTimeOffset>("Created");

                    b.Property<string>("EmailAddress");

                    b.Property<DateTimeOffset>("Modified");

                    b.Property<bool>("ShouldSendEmail");

                    b.Property<bool>("ShouldSendPush");

                    b.Property<bool>("ShouldSendSms");

                    b.Property<string>("SmsNumber");

                    b.HasKey("UserPreferencesId");

                    b.HasIndex("AppUserId")
                        .IsUnique();

                    b.ToTable("UserPreferences");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("StockAlerts.Data.Model.AlertCriteria", b =>
                {
                    b.HasOne("StockAlerts.Data.Model.AlertDefinition", "AlertDefinition")
                        .WithOne("RootCriteria")
                        .HasForeignKey("StockAlerts.Data.Model.AlertCriteria", "AlertDefinitionId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("StockAlerts.Data.Model.AlertCriteria", "ParentCriteria")
                        .WithMany("ChildrenCriteria")
                        .HasForeignKey("ParentCriteriaId");
                });

            modelBuilder.Entity("StockAlerts.Data.Model.AlertDefinition", b =>
                {
                    b.HasOne("StockAlerts.Data.Model.AppUser", "AppUser")
                        .WithMany("AlertDefinitions")
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("StockAlerts.Data.Model.Stock", "Stock")
                        .WithMany()
                        .HasForeignKey("StockId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("StockAlerts.Data.Model.AlertTriggerHistory", b =>
                {
                    b.HasOne("StockAlerts.Data.Model.AlertDefinition", "AlertDefinition")
                        .WithMany("AlertTriggerHistories")
                        .HasForeignKey("AlertDefinitionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("StockAlerts.Data.Model.RefreshToken", b =>
                {
                    b.HasOne("StockAlerts.Data.Model.AppUser")
                        .WithMany("RefreshTokens")
                        .HasForeignKey("AppUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("StockAlerts.Data.Model.UserPreferences", b =>
                {
                    b.HasOne("StockAlerts.Data.Model.AppUser", "AppUser")
                        .WithOne("UserPreferences")
                        .HasForeignKey("StockAlerts.Data.Model.UserPreferences", "AppUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
