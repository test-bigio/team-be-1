using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BigioHrServices.Migrations
{
    public partial class SimplifyLeaveData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeaveStart",
                table: "Leaves");

            migrationBuilder.DropColumn(
                name: "TotalLeaveInDays",
                table: "Leaves");

            migrationBuilder.AddColumn<DateOnly>(
                name: "LeaveDate",
                table: "Leaves",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeaveDate",
                table: "Leaves");

            migrationBuilder.AddColumn<DateTime>(
                name: "LeaveStart",
                table: "Leaves",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "TotalLeaveInDays",
                table: "Leaves",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
