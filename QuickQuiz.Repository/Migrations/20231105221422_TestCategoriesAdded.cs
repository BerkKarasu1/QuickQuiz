using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickQuiz.Repository.Migrations
{
    /// <inheritdoc />
    public partial class TestCategoriesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TestCategorys",
                table: "Tests",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TestCategorys",
                table: "Tests");
        }
    }
}
