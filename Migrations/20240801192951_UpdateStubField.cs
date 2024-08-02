using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhatIfSports.Migrations
{
    /// <inheritdoc />
    public partial class UpdateStubField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "stub",
                table: "Players");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "stub",
                table: "Players",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
