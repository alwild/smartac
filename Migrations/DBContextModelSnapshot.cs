﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using smartacfe.Data;

namespace smartacfe.Migrations
{
    [DbContext(typeof(DBContext))]
    partial class DBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("smartacfe.Models.ACDevice", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccessKey");

                    b.Property<string>("FirmwareVersion")
                        .IsRequired();

                    b.Property<DateTime>("RegistrationDate");

                    b.Property<string>("SerialNumber")
                        .IsRequired();

                    b.HasKey("ID");

                    b.HasIndex("SerialNumber")
                        .IsUnique();

                    b.ToTable("ACDevices");
                });

            modelBuilder.Entity("smartacfe.Models.ACDeviceAlert", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ACDeviceID");

                    b.Property<DateTime>("AlertDateTime");

                    b.Property<int>("AlertType");

                    b.Property<bool>("Cleared");

                    b.Property<DateTime>("ReadingDateTime");

                    b.HasKey("ID");

                    b.HasIndex("ACDeviceID");

                    b.ToTable("ACDeviceAlerts");
                });

            modelBuilder.Entity("smartacfe.Models.ACDeviceReading", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ACDeviceID");

                    b.Property<decimal>("COLevel");

                    b.Property<string>("HealthStatus");

                    b.Property<decimal>("Humidity");

                    b.Property<DateTime>("LoggedDateTime");

                    b.Property<DateTime>("ReadingDateTime");

                    b.Property<decimal>("Temperature");

                    b.HasKey("ID");

                    b.HasIndex("ACDeviceID");

                    b.ToTable("AcDeviceReadings");
                });

            modelBuilder.Entity("smartacfe.Models.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("Password");

                    b.Property<string>("Username");

                    b.HasKey("ID");

                    b.HasIndex("Username")
                        .IsUnique()
                        .HasFilter("[Username] IS NOT NULL");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("smartacfe.Models.ACDeviceAlert", b =>
                {
                    b.HasOne("smartacfe.Models.ACDevice", "Device")
                        .WithMany()
                        .HasForeignKey("ACDeviceID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("smartacfe.Models.ACDeviceReading", b =>
                {
                    b.HasOne("smartacfe.Models.ACDevice", "ACDevice")
                        .WithMany("ACDeviceReading")
                        .HasForeignKey("ACDeviceID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
