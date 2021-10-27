using Microsoft.EntityFrameworkCore.Migrations;

namespace Devmoba.ToolManager.Migrations
{
    public partial class Edit_Unique_Tool_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppTools_AppId",
                table: "AppTools");

            migrationBuilder.CreateIndex(
                name: "IX_AppTools_AppId_ClientId",
                table: "AppTools",
                columns: new[] { "AppId", "ClientId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppTools_AppId_ClientId",
                table: "AppTools");

            migrationBuilder.CreateIndex(
                name: "IX_AppTools_AppId",
                table: "AppTools",
                column: "AppId",
                unique: true);
        }
    }
}
