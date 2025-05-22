using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolV01.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class alterProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProductCategories",
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
                    table.PrimaryKey("PK_ProductCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCategories_ProductCategories_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "ProductCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Products",
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
                    ProductParentCategoryId = table.Column<int>(type: "int", nullable: true),
                    ProductSubCategoryId = table.Column<int>(type: "int", nullable: true),
                    ProductSubSubCategoryId = table.Column<int>(type: "int", nullable: true),
                    ProductSubSubSubCategoryId = table.Column<int>(type: "int", nullable: true),
                    ProductDefaultCategoryId = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    IsRecent = table.Column<bool>(type: "bit", nullable: false),
                    Plan = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductImageUrl1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductImageUrl2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductImageUrl3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_ProductCategories_ProductDefaultCategoryId",
                        column: x => x.ProductDefaultCategoryId,
                        principalTable: "ProductCategories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Products_ProductCategories_ProductParentCategoryId",
                        column: x => x.ProductParentCategoryId,
                        principalTable: "ProductCategories",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ProductOffers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_ProductOffers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductOffers_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_Deleted",
                table: "ProductCategories",
                column: "Deleted");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_ParentCategoryId",
                table: "ProductCategories",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOffers_Deleted",
                table: "ProductOffers",
                column: "Deleted");

            migrationBuilder.CreateIndex(
                name: "IX_ProductOffers_ProductId",
                table: "ProductOffers",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Deleted",
                table: "Products",
                column: "Deleted");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductDefaultCategoryId",
                table: "Products",
                column: "ProductDefaultCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductParentCategoryId",
                table: "Products",
                column: "ProductParentCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductOffers");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "ProductCategories");
        }
    }
}
