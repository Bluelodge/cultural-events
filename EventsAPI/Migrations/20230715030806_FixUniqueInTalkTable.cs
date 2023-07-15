using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventsAPI.Migrations
{
    public partial class FixUniqueInTalkTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Talk_Title",
                table: "Talk");

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "Talk",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Talk_Title_EventId",
                table: "Talk",
                columns: new[] { "Title", "EventId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Talk_Title_EventId",
                table: "Talk");

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "Talk",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Talk_Title",
                table: "Talk",
                column: "Title",
                unique: true);
        }
    }
}
