using Microsoft.EntityFrameworkCore.Migrations;

namespace LegoM.Data.Migrations
{
    public partial class ProductsImagesTableOnDeleteCascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsImages_Products_ProductId",
                table: "ProductsImages");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsImages_Products_ProductId",
                table: "ProductsImages",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductsImages_Products_ProductId",
                table: "ProductsImages");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductsImages_Products_ProductId",
                table: "ProductsImages",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
