using Microsoft.EntityFrameworkCore.Migrations;

namespace LegoM.Data.Migrations
{
    public partial class ReviewsQuestionsAnswersCommentsTablesColumnIsPublic : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Reviews",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Questions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Comments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Answers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Questions");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Answers");
        }
    }
}
