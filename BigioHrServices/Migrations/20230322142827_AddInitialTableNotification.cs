using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BigioHrServices.Migrations
{
    public partial class AddInitialTableNotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    nik = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    body = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    is_read = table.Column<bool>(type: "boolean", nullable: false),
                    read_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.id);
                    table.ForeignKey(
                        name: "FK_Notifications_Employees_nik",
                        column: x => x.nik,
                        principalTable: "Employees",
                        principalColumn: "nik",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_nik",
                table: "Notifications",
                column: "nik");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Notifications");
        }
    }
}
