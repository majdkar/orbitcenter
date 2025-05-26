using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolV01.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class alterPerson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdentifierImageUrl",
                table: "People",
                newName: "Qualification");

            migrationBuilder.RenameColumn(
                name: "FullNameEn",
                table: "People",
                newName: "Mobile2");

            migrationBuilder.RenameColumn(
                name: "FullNameAr",
                table: "People",
                newName: "Mobile1");

            migrationBuilder.AddColumn<int>(
                name: "ClassificationId",
                table: "People",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "People",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Job",
                table: "People",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Classifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classifications", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_People_ClassificationId",
                table: "People",
                column: "ClassificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Classifications_Deleted",
                table: "Classifications",
                column: "Deleted");

            migrationBuilder.AddForeignKey(
                name: "FK_People_Classifications_ClassificationId",
                table: "People",
                column: "ClassificationId",
                principalTable: "Classifications",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_People_Classifications_ClassificationId",
                table: "People");

            migrationBuilder.DropTable(
                name: "Classifications");

            migrationBuilder.DropIndex(
                name: "IX_People_ClassificationId",
                table: "People");

            migrationBuilder.DropColumn(
                name: "ClassificationId",
                table: "People");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "People");

            migrationBuilder.DropColumn(
                name: "Job",
                table: "People");

            migrationBuilder.RenameColumn(
                name: "Qualification",
                table: "People",
                newName: "IdentifierImageUrl");

            migrationBuilder.RenameColumn(
                name: "Mobile2",
                table: "People",
                newName: "FullNameEn");

            migrationBuilder.RenameColumn(
                name: "Mobile1",
                table: "People",
                newName: "FullNameAr");
        }
    }
}
