using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickQuiz.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddedExamRating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "ExamRating",
                table: "ExamResults",
                type: "smallint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExamRating",
                table: "ExamResults");
        }
    }
}
