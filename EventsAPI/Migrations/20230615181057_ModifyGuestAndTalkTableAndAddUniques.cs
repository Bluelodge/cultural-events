using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventsAPI.Migrations
{
    public partial class ModifyGuestAndTalkTableAndAddUniques : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventGuest_Host_GuestId",
                table: "EventGuest");

            migrationBuilder.DropForeignKey(
                name: "FK_Talk_Category_CategoryId",
                table: "Talk");

            migrationBuilder.DropForeignKey(
                name: "FK_TalkGuest_Host_GuestId",
                table: "TalkGuest");

            migrationBuilder.DropIndex(
                name: "IX_Attendee_UserName",
                table: "Attendee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Host",
                table: "Host");

            migrationBuilder.RenameTable(
                name: "Host",
                newName: "Guest");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Talk",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "Talk",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "Guest",
                type: "nvarchar(400)",
                maxLength: 400,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Guest",
                table: "Guest",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Talk_EventId",
                table: "Talk",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Talk_Title",
                table: "Talk",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Event_Title",
                table: "Event",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Category_Name",
                table: "Category",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attendee_UserName_EmailAddress",
                table: "Attendee",
                columns: new[] { "UserName", "EmailAddress" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Guest_FullName_Position",
                table: "Guest",
                columns: new[] { "FullName", "Position" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EventGuest_Guest_GuestId",
                table: "EventGuest",
                column: "GuestId",
                principalTable: "Guest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Talk_Category_CategoryId",
                table: "Talk",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Talk_Event_EventId",
                table: "Talk",
                column: "EventId",
                principalTable: "Event",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TalkGuest_Guest_GuestId",
                table: "TalkGuest",
                column: "GuestId",
                principalTable: "Guest",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventGuest_Guest_GuestId",
                table: "EventGuest");

            migrationBuilder.DropForeignKey(
                name: "FK_Talk_Category_CategoryId",
                table: "Talk");

            migrationBuilder.DropForeignKey(
                name: "FK_Talk_Event_EventId",
                table: "Talk");

            migrationBuilder.DropForeignKey(
                name: "FK_TalkGuest_Guest_GuestId",
                table: "TalkGuest");

            migrationBuilder.DropIndex(
                name: "IX_Talk_EventId",
                table: "Talk");

            migrationBuilder.DropIndex(
                name: "IX_Talk_Title",
                table: "Talk");

            migrationBuilder.DropIndex(
                name: "IX_Event_Title",
                table: "Event");

            migrationBuilder.DropIndex(
                name: "IX_Category_Name",
                table: "Category");

            migrationBuilder.DropIndex(
                name: "IX_Attendee_UserName_EmailAddress",
                table: "Attendee");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Guest",
                table: "Guest");

            migrationBuilder.DropIndex(
                name: "IX_Guest_FullName_Position",
                table: "Guest");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "Talk");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "Guest");

            migrationBuilder.RenameTable(
                name: "Guest",
                newName: "Host");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Talk",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Host",
                table: "Host",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Attendee_UserName",
                table: "Attendee",
                column: "UserName",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EventGuest_Host_GuestId",
                table: "EventGuest",
                column: "GuestId",
                principalTable: "Host",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Talk_Category_CategoryId",
                table: "Talk",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TalkGuest_Host_GuestId",
                table: "TalkGuest",
                column: "GuestId",
                principalTable: "Host",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
