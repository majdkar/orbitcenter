using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolV01.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class alterPageSeo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PageSeo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PageId = table.Column<int>(type: "int", nullable: false),
                    MetaTitleAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaTitleEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaTitleGe = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaNameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaNameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaNameGe = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaUrlAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaUrlEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaUrlGe = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaKeywordsAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaKeywordsEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaKeywordsGe = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaDescriptionsAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaDescriptionsEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaDescriptionsGe = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageAlt1Ar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageAlt1En = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageAlt1Ge = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageAlt2Ar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageAlt2En = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageAlt2Ge = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageAlt3Ar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageAlt3En = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageAlt3Ge = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageAlt4Ar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageAlt4En = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageAlt4Ge = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MetaRobots = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageSeo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PageSeo_Pages_PageId",
                        column: x => x.PageId,
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PageSeo_Deleted",
                table: "PageSeo",
                column: "Deleted");

            migrationBuilder.CreateIndex(
                name: "IX_PageSeo_PageId",
                table: "PageSeo",
                column: "PageId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PageSeo");
        }
    }
}
