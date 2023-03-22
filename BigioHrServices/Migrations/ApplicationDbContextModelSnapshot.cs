﻿// <auto-generated />
using System;
using BigioHrServices.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BigioHrServices.Db.Entities
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

            modelBuilder.Entity("BigioHrServices.Db.Entities.Delegation", b =>
                {
                    b.Property<string>("NIK")
                        .HasColumnType("text")
                        .HasColumnName("nik");

                    b.Property<string>("ParentNIK")
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

            modelBuilder.Entity("BigioHrServices.Db.Entities.Employee", b =>
                {
                    b.Property<string>("NIK")
                        .HasColumnType("text")
                        .HasColumnName("nik");

                    b.Property<long?>("CreatedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("created_by");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_date");

                    b.Property<string>("DigitalSignature")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("digital_signature");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("is_active");

                    b.Property<bool>("IsOnLeave")
                        .HasColumnType("boolean")
                        .HasColumnName("is_on_leave");

                    b.Property<int>("JatahCuti")
                        .HasColumnType("integer")
                        .HasColumnName("jatah_cuti");

                    b.Property<DateOnly>("JoinDate")
                        .HasColumnType("date")
                        .HasColumnName("join_date");

                    b.Property<DateTime>("LastUpdatePassword")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("last_update_password");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("OTP")
                        .HasColumnType("text")
                        .HasColumnName("otp");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.Property<string>("PositionCode")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("position_code");

                    b.Property<long>("PositionID")
                        .HasColumnType("bigint")
                        .HasColumnName("position_id");

                    b.Property<string>("Sex")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("sex");

                    b.Property<long?>("UpdatedBy")
                        .HasColumnType("bigint")
                        .HasColumnName("updated_by");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("updated_date");

                    b.Property<string>("WorkLength")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("work_length");

                    b.HasKey("NIK");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("BigioHrServices.Db.Entities.Leave", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("DelegatedStafNIK")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("LeaveStart")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ReviewerNIK")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("StafNIK")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<int>("TotalLeaveInDays")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("Leaves");
                });

            modelBuilder.Entity("BigioHrServices.Db.Entities.LogActivity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<string>("Activity")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("activity");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("date");

                    b.Property<string>("Modul")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("modul");

                    b.HasKey("ID");

                    b.ToTable("LogActivities");
                });

            modelBuilder.Entity("BigioHrServices.Db.Entities.Notification", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("body");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_date");

                    b.Property<string>("Data")
                        .IsRequired()
                        .HasMaxLength(8000)
                        .HasColumnType("character varying(8000)")
                        .HasColumnName("data");

                    b.Property<bool>("IsRead")
                        .HasColumnType("boolean")
                        .HasColumnName("is_read");

                    b.Property<DateTime?>("ReadDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("read_date");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("title");

                    b.HasKey("Id");

                    b.ToTable("Notifications");
                });

            modelBuilder.Entity("BigioHrServices.Db.Entities.Position", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("text")
                        .HasColumnName("code");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("is_active");

                    b.Property<string>("Level")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("level");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Code");

                    b.ToTable("Positions");
                });
#pragma warning restore 612, 618
        }
    }
}
