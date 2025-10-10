using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolV01.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class alterService234243 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ClientType",
                table: "ProductOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientType",
                table: "CourseOrders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientType",
                table: "ProductOrders");

            migrationBuilder.DropColumn(
                name: "ClientType",
                table: "CourseOrders");
        }
    }
}
