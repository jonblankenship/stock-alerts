﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StockAlerts.Data;

namespace StockAlerts.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

                    b.Property<int>("ComparisonOperator");

                    b.Property<decimal>("PriceLevel");

                    b.Property<int>("Status");

                    b.Property<string>("Symbol");

                    b.Property<int>("Type");

                    b.HasKey("AlertDefinitionId");

                    b.ToTable("AlertDefinitions");
                });
#pragma warning restore 612, 618
        }
    }
}
