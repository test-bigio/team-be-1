using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BigioHrServices.Migrations
{
    public partial class UpdateEmployeeColumnName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Sex",
                table: "Employees",
                newName: "sex");

            migrationBuilder.RenameColumn(
                name: "Position",
                table: "Employees",
                newName: "position");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Employees",
                newName: "password");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Employees",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "NIK",
                table: "Employees",
                newName: "nik");

            migrationBuilder.RenameColumn(
                name: "WorkLength",
                table: "Employees",
                newName: "work_length");

            migrationBuilder.RenameColumn(
                name: "JoinDate",
                table: "Employees",
                newName: "join_date");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Employees",
                newName: "is_active");

            migrationBuilder.RenameColumn(
                name: "DigitalSignature",
                table: "Employees",
                newName: "digital_signature");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "sex",
                table: "Employees",
                newName: "Sex");

            migrationBuilder.RenameColumn(
                name: "position",
                table: "Employees",
                newName: "Position");

            migrationBuilder.RenameColumn(
                name: "password",
                table: "Employees",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Employees",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "nik",
                table: "Employees",
                newName: "NIK");

            migrationBuilder.RenameColumn(
                name: "work_length",
                table: "Employees",
                newName: "WorkLength");

            migrationBuilder.RenameColumn(
                name: "join_date",
                table: "Employees",
                newName: "JoinDate");

            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "Employees",
                newName: "IsActive");

            migrationBuilder.RenameColumn(
                name: "digital_signature",
                table: "Employees",
                newName: "DigitalSignature");
        }
    }
}
