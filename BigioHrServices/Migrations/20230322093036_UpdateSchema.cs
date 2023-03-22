using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BigioHrServices.Migrations
{
    public partial class UpdateSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Delegations",
                table: "Delegations");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "Delegations");

            migrationBuilder.RenameColumn(
                name: "NIK",
                table: "Delegations",
                newName: "nik");

            migrationBuilder.RenameColumn(
                name: "ParentNIK",
                table: "Delegations",
                newName: "parent_nik");

            migrationBuilder.AddColumn<int>(
                name: "TotalLeaveInDays",
                table: "Leaves",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Delegations",
                table: "Delegations",
                column: "nik");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Delegations",
                table: "Delegations");

            migrationBuilder.DropColumn(
                name: "TotalLeaveInDays",
                table: "Leaves");

            migrationBuilder.RenameColumn(
                name: "nik",
                table: "Delegations",
                newName: "NIK");

            migrationBuilder.RenameColumn(
                name: "parent_nik",
                table: "Delegations",
                newName: "ParentNIK");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "Delegations",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Delegations",
                table: "Delegations",
                column: "ID");
        }
    }
}
