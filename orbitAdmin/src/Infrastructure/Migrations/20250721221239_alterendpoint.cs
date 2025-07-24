using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolV01.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class alterendpoint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EndpointAr",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EndpointEn",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EndpointGe",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EndpointAr",
                table: "Pages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EndpointEn",
                table: "Pages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EndpointGe",
                table: "Pages",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EndpointAr",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EndpointEn",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EndpointGe",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EndpointAr",
                table: "Blocks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EndpointEn",
                table: "Blocks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EndpointGe",
                table: "Blocks",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndpointAr",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "EndpointEn",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "EndpointGe",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "EndpointAr",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "EndpointEn",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "EndpointGe",
                table: "Pages");

            migrationBuilder.DropColumn(
                name: "EndpointAr",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "EndpointEn",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "EndpointGe",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "EndpointAr",
                table: "Blocks");

            migrationBuilder.DropColumn(
                name: "EndpointEn",
                table: "Blocks");

            migrationBuilder.DropColumn(
                name: "EndpointGe",
                table: "Blocks");
        }
    }
}
