using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FunDooNotesC_.DataLayer.Migrations
{
    public partial class FixCascadeDeleteIssue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NoteLabels_Labels_LabelId",
                table: "NoteLabels");

            migrationBuilder.DropForeignKey(
                name: "FK_NoteLabels_Notes_NoteId",
                table: "NoteLabels");

            migrationBuilder.AddForeignKey(
                name: "FK_NoteLabels_Labels_LabelId",
                table: "NoteLabels",
                column: "LabelId",
                principalTable: "Labels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NoteLabels_Notes_NoteId",
                table: "NoteLabels",
                column: "NoteId",
                principalTable: "Notes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NoteLabels_Labels_LabelId",
                table: "NoteLabels");

            migrationBuilder.DropForeignKey(
                name: "FK_NoteLabels_Notes_NoteId",
                table: "NoteLabels");

            migrationBuilder.AddForeignKey(
                name: "FK_NoteLabels_Labels_LabelId",
                table: "NoteLabels",
                column: "LabelId",
                principalTable: "Labels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NoteLabels_Notes_NoteId",
                table: "NoteLabels",
                column: "NoteId",
                principalTable: "Notes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
