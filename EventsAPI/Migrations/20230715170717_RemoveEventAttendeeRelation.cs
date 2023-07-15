using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventsAPI.Migrations
{
    public partial class RemoveEventAttendeeRelation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventAttendee");

            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "Talk",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "EventId",
                table: "Talk",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateTable(
                name: "EventAttendee",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "int", nullable: false),
                    AttendeeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventAttendee", x => new { x.EventId, x.AttendeeId });
                    table.ForeignKey(
                        name: "FK_EventAttendee_Attendee_AttendeeId",
                        column: x => x.AttendeeId,
                        principalTable: "Attendee",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventAttendee_Event_EventId",
                        column: x => x.EventId,
                        principalTable: "Event",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventAttendee_AttendeeId",
                table: "EventAttendee",
                column: "AttendeeId");
        }
    }
}
