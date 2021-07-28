using Microsoft.EntityFrameworkCore.Migrations;

namespace Devmoba.ToolManager.Migrations
{
    public partial class Remove_ClientStatus_inClientMachine_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientStatus",
                table: "AppClientMachines");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientStatus",
                table: "AppClientMachines",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
