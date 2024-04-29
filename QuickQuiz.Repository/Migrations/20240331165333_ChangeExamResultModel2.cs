using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickQuiz.Repository.Migrations
{
    /// <inheritdoc />
    public partial class ChangeExamResultModel2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Result",
                table: "ExamResults",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "ExamResults",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartTime",
                table: "ExamResults",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "ExamResults");

            migrationBuilder.DropColumn(
                name: "StartTime",
                table: "ExamResults");

            migrationBuilder.AlterColumn<float>(
                name: "Result",
                table: "ExamResults",
                type: "real",
                nullable: false,
                defaultValue: 0f,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);
        }
    }
}
