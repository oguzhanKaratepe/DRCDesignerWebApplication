using Microsoft.EntityFrameworkCore.Migrations;

namespace DRCDesignerWebApplication.Migrations
{
    public partial class SubdomainVersionUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DexmoVersion",
                table: "SubdomainVersions",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DexmoVersion",
                table: "SubdomainVersions");
        }
    }
}
