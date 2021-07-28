using Microsoft.EntityFrameworkCore.Migrations;

namespace Devmoba.ToolManager.Migrations
{
    public partial class Add_Full_Text_Indexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
              sql: "CREATE FULLTEXT CATALOG ftCatalog_AppTools AS DEFAULT",
              suppressTransaction: true);

            migrationBuilder.Sql(
                sql: "CREATE FULLTEXT INDEX ON AppTools([Name]) KEY INDEX PK_AppTools",
                suppressTransaction: true);

            migrationBuilder.Sql(
               sql: "CREATE FULLTEXT CATALOG ftCatalog_AppScripts AS DEFAULT",
               suppressTransaction: true);

            migrationBuilder.Sql(
                sql: "CREATE FULLTEXT INDEX ON AppScripts([Name]) KEY INDEX PK_AppScripts",
                suppressTransaction: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
             sql: "DROP FULLTEXT INDEX ON AppTools",
             suppressTransaction: true);

            migrationBuilder.Sql(
                sql: "DROP FULLTEXT CATALOG ftCatalog_AppTools",
                suppressTransaction: true);

            migrationBuilder.Sql(
               sql: "DROP FULLTEXT INDEX ON AppScripts",
               suppressTransaction: true);

            migrationBuilder.Sql(
                sql: "DROP FULLTEXT CATALOG ftCatalog_AppScripts",
                suppressTransaction: true);
        }
    }
}
