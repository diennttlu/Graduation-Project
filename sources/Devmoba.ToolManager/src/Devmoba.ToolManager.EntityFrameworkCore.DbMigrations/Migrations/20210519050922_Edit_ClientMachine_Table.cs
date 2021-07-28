using Microsoft.EntityFrameworkCore.Migrations;

namespace Devmoba.ToolManager.Migrations
{
    public partial class Edit_ClientMachine_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppTools_AppClientMachines_ClientId",
                table: "AppTools");

            migrationBuilder.DropColumn(
                name: "ClientStatus",
                table: "AppClientMachines");

            migrationBuilder.AddForeignKey(
                name: "FK_AppTools_AppClientMachines_ClientId",
                table: "AppTools",
                column: "ClientId",
                principalTable: "AppClientMachines",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppTools_AppClientMachines_ClientId",
                table: "AppTools");

            migrationBuilder.AddColumn<int>(
                name: "ClientStatus",
                table: "AppClientMachines",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_AppTools_AppClientMachines_ClientId",
                table: "AppTools",
                column: "ClientId",
                principalTable: "AppClientMachines",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
