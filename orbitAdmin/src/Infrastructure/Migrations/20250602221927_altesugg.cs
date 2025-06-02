using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolV01.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class altesugg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AppointmentDate",
                table: "Suggestions",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AppointmentDate",
                table: "Suggestions");
        }
    }
}
