using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolV01.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addCourse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CourseCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameGe = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionAr1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionAr2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionAr3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionAr4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEn1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEn2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEn3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEn4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionGe1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionGe2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionGe3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionGe4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentCategoryId = table.Column<int>(type: "int", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ImageDataURL1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageDataURL2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageDataURL3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseCategories_CourseCategories_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "CourseCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameEn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NameGe = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEn1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEn2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEn3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionEn4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionAr1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionAr2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionAr3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionAr4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionGe1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionGe2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionGe3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionGe4 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CourseParentCategoryId = table.Column<int>(type: "int", nullable: true),
                    CourseSubCategoryId = table.Column<int>(type: "int", nullable: true),
                    CourseSubSubCategoryId = table.Column<int>(type: "int", nullable: true),
                    CourseSubSubSubCategoryId = table.Column<int>(type: "int", nullable: true),
                    CourseDefaultCategoryId = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    IsRecent = table.Column<bool>(type: "bit", nullable: false),
                    Plan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CourseImageUrl1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CourseImageUrl2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CourseImageUrl3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Keywords = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SeoDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Courses_CourseCategories_CourseDefaultCategoryId",
                        column: x => x.CourseDefaultCategoryId,
                        principalTable: "CourseCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Courses_CourseCategories_CourseParentCategoryId",
                        column: x => x.CourseParentCategoryId,
                        principalTable: "CourseCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CourseOffers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    DiscountRatio = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    NewPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseOffers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseOffers_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseCategories_Deleted",
                table: "CourseCategories",
                column: "Deleted");

            migrationBuilder.CreateIndex(
                name: "IX_CourseCategories_ParentCategoryId",
                table: "CourseCategories",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseOffers_CourseId",
                table: "CourseOffers",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseOffers_Deleted",
                table: "CourseOffers",
                column: "Deleted");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CourseDefaultCategoryId",
                table: "Courses",
                column: "CourseDefaultCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CourseParentCategoryId",
                table: "Courses",
                column: "CourseParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_Deleted",
                table: "Courses",
                column: "Deleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseOffers");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "CourseCategories");
        }
    }
}
