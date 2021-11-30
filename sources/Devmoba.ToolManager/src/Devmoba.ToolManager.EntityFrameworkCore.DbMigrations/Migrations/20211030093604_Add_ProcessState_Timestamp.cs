using Microsoft.EntityFrameworkCore.Migrations;

namespace Devmoba.ToolManager.Migrations
{
    public partial class Add_ProcessState_Timestamp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProcessState",
                table: "AppTools",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "Timestamp",
                table: "AppClientMachines",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProcessState",
                table: "AppTools");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "AppClientMachines");
        }
    }
}
