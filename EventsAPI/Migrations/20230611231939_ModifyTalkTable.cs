using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventsAPI.Migrations
{
    public partial class ModifyTalkTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Talk",
                newName: "Title");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Talk",
                newName: "Name");
        }
    }
}
