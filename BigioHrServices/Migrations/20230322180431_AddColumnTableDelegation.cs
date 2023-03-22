using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BigioHrServices.Migrations
{
    public partial class AddColumnTableDelegation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Delegations",
                table: "Delegations");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Delegations",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "priority",
                table: "Delegations",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Delegations",
                table: "Delegations",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Delegations",
                table: "Delegations");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Delegations");

            migrationBuilder.DropColumn(
                name: "priority",
                table: "Delegations");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Delegations",
                table: "Delegations",
                column: "nik");
        }
    }
}
