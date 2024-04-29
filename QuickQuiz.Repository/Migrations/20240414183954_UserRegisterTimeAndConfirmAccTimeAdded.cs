using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuickQuiz.Repository.Migrations
{
    /// <inheritdoc />
    public partial class UserRegisterTimeAndConfirmAccTimeAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AccountConfirmTime",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RegisterTime",
                table: "AspNetUsers",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountConfirmTime",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RegisterTime",
                table: "AspNetUsers");
        }
    }
}
