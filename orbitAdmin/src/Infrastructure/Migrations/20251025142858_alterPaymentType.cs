using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SchoolV01.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class alterPaymentType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PayTypeId",
                table: "ProductOrders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentTransactionNumber",
                table: "ProductOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PayTypeId",
                table: "CourseOrders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentTransactionNumber",
                table: "CourseOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PayTypes",
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
                    table.PrimaryKey("PK_PayTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductOrders_PayTypeId",
                table: "ProductOrders",
                column: "PayTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseOrders_PayTypeId",
                table: "CourseOrders",
                column: "PayTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PayTypes_Deleted",
                table: "PayTypes",
                column: "Deleted");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseOrders_PayTypes_PayTypeId",
                table: "CourseOrders",
                column: "PayTypeId",
                principalTable: "PayTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductOrders_PayTypes_PayTypeId",
                table: "ProductOrders",
                column: "PayTypeId",
                principalTable: "PayTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseOrders_PayTypes_PayTypeId",
                table: "CourseOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductOrders_PayTypes_PayTypeId",
                table: "ProductOrders");

            migrationBuilder.DropTable(
                name: "PayTypes");

            migrationBuilder.DropIndex(
                name: "IX_ProductOrders_PayTypeId",
                table: "ProductOrders");

            migrationBuilder.DropIndex(
                name: "IX_CourseOrders_PayTypeId",
                table: "CourseOrders");

            migrationBuilder.DropColumn(
                name: "PayTypeId",
                table: "ProductOrders");

            migrationBuilder.DropColumn(
                name: "PaymentTransactionNumber",
                table: "ProductOrders");

            migrationBuilder.DropColumn(
                name: "PayTypeId",
                table: "CourseOrders");

            migrationBuilder.DropColumn(
                name: "PaymentTransactionNumber",
                table: "CourseOrders");
        }
    }
}
