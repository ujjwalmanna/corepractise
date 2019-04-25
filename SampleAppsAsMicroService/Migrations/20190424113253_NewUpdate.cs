using Microsoft.EntityFrameworkCore.Migrations;

namespace SampleAppsAsMicroService.Migrations
{
    public partial class NewUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_JobRoles_RoleId",
                table: "Employees");

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "Employees",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_JobRoles_RoleId",
                table: "Employees",
                column: "RoleId",
                principalTable: "JobRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_JobRoles_RoleId",
                table: "Employees");

            migrationBuilder.AlterColumn<int>(
                name: "RoleId",
                table: "Employees",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_JobRoles_RoleId",
                table: "Employees",
                column: "RoleId",
                principalTable: "JobRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
