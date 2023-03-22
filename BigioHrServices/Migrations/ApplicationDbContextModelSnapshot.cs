﻿// <auto-generated />
using System;
using BigioHrServices.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BigioHrServices.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BigioHrServices.Db.Entities.Employee", b =>
                {
                    b.Property<string>("NIK")
                        .HasColumnType("text")
                        .HasColumnName("nik");

                    b.Property<string>("DigitalSignature")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("digital_signature");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("is_active");

                    b.Property<DateOnly>("JoinDate")
                        .HasColumnType("date")
                        .HasColumnName("join_date");

                    b.Property<DateTime>("LastUpdatePassword")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_update_password");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.Property<string>("Position")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("position");

                    b.Property<string>("Sex")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("sex");

                    b.Property<string>("WorkLength")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("work_length");

                    b.HasKey("NIK");

                    b.ToTable("Employees");
                });
#pragma warning restore 612, 618
        }
    }
}
