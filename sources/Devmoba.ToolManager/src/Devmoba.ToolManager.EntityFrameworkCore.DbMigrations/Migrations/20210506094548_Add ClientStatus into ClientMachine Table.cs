using Microsoft.EntityFrameworkCore.Migrations;

namespace Devmoba.ToolManager.Migrations
{
    public partial class AddClientStatusintoClientMachineTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClientStatus",
                table: "AppClientMachines",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientStatus",
                table: "AppClientMachines");
        }
    }
}
