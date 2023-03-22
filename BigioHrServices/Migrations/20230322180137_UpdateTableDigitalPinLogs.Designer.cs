﻿// <auto-generated />
using System;
using BigioHrServices.Db;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BigioHrServices.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230322180137_UpdateTableDigitalPinLogs")]
    partial class UpdateTableDigitalPinLogs
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BigioHrServices.Db.Entities.AuditModul", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Activity")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Detail")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("IpAddress")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Module")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("AuditModuls");
                });

            modelBuilder.Entity("BigioHrServices.Db.Entities.Delegation", b =>
                {
                    b.Property<string>("NIK")
                        .HasColumnType("text")
                        .HasColumnName("nik");

                    b.Property<string>("ParentNIK")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("parent_nik");

                    b.HasKey("NIK");

                    b.ToTable("Delegations");
                });

            modelBuilder.Entity("BigioHrServices.Db.Entities.DigitalPinLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Pin")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("pin");

                    b.Property<string>("StaffId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("staff_id");

                    b.HasKey("Id");

                    b.ToTable("DigitalPinLogs");
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

                    b.Property<DateOnly>("LeaveDate")
                        .HasColumnType("date");

                    b.Property<string>("ReviewerNIK")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("StafNIK")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("Leaves");
                });

            modelBuilder.Entity("BigioHrServices.Db.Entities.Notification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("body");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("created_date");

                    b.Property<bool>("IsRead")
                        .HasColumnType("boolean")
                        .HasColumnName("is_read");

                    b.Property<string>("Nik")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("nik");

                    b.Property<DateTime?>("ReadDate")
                        .HasColumnType("timestamp without time zone")
                        .HasColumnName("read_date");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("title");

                    b.HasKey("Id");

                    b.HasIndex("Nik");

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

            modelBuilder.Entity("BigioHrServices.Db.Entities.Notification", b =>
                {
                    b.HasOne("BigioHrServices.Db.Entities.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("Nik")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });
#pragma warning restore 612, 618
        }
    }
}