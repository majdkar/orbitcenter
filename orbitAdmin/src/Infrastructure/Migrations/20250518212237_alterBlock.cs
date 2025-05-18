using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolV01.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class alterBlock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParentId",
                table: "Blocks",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Blocks_ParentId",
                table: "Blocks",
                column: "ParentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blocks_Blocks_ParentId",
                table: "Blocks",
                column: "ParentId",
                principalTable: "Blocks",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blocks_Blocks_ParentId",
                table: "Blocks");

            migrationBuilder.DropIndex(
                name: "IX_Blocks_ParentId",
                table: "Blocks");

            migrationBuilder.DropColumn(
                name: "ParentId",
                table: "Blocks");
        }
    }
}
