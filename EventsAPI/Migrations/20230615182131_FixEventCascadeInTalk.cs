using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventsAPI.Migrations
{
    public partial class FixEventCascadeInTalk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Talk_Event_EventId",
                table: "Talk");

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "Talk",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Talk_Event_EventId",
                table: "Talk",
                column: "EventId",
                principalTable: "Event",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Talk_Event_EventId",
                table: "Talk");

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "Talk",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Talk_Event_EventId",
                table: "Talk",
                column: "EventId",
                principalTable: "Event",
                principalColumn: "Id");
        }
    }
}
