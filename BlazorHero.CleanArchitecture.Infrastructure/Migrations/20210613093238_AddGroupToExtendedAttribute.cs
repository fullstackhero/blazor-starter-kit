using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorHero.CleanArchitecture.Infrastructure.Migrations
{
    public partial class AddGroupToExtendedAttribute : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Group",
                table: "DocumentExtendedAttributes",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Group",
                table: "DocumentExtendedAttributes");
        }
    }
}
