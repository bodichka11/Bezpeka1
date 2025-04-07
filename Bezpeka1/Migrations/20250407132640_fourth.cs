using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bezpeka1.Migrations
{
    /// <inheritdoc />
    public partial class fourth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "CaptchaImages");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "CaptchaImages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "CaptchaImages",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "CaptchaImages");

            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "CaptchaImages");

            migrationBuilder.AddColumn<string>(
                name: "ImagePath",
                table: "CaptchaImages",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
