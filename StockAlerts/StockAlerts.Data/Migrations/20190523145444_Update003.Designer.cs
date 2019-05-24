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
    [Migration("20190523145444_Update003")]
    partial class Update003
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("StockAlerts.Data.Model.AlertDefinition", b =>
                {
                    b.Property<Guid>("AlertDefinitionId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid>("AppUserId");

                    b.Property<int>("ComparisonOperator");

                    b.Property<DateTimeOffset>("Created");

                    b.Property<DateTimeOffset>("Modified");

                    b.Property<decimal>("PriceLevel");

                    b.Property<int>("Status");

                    b.Property<Guid>("StockId");

                    b.Property<int>("Type");

                    b.HasKey("AlertDefinitionId");

                    b.HasIndex("AppUserId");

                    b.HasIndex("StockId");

                    b.ToTable("AlertDefinitions");
                });

            modelBuilder.Entity("StockAlerts.Data.Model.AppUser", b =>
                {
                    b.Property<Guid>("AppUserId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("Created");

                    b.Property<DateTimeOffset>("Modified");

                    b.Property<Guid>("UserId");

                    b.HasKey("AppUserId");

                    b.ToTable("AppUsers");
                });

            modelBuilder.Entity("StockAlerts.Data.Model.Stock", b =>
                {
                    b.Property<Guid>("StockId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("Created");

                    b.Property<decimal>("LastPrice");

                    b.Property<DateTimeOffset>("LastTime");

                    b.Property<DateTimeOffset>("Modified");

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
