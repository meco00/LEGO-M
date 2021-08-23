using Microsoft.EntityFrameworkCore.Migrations;

namespace LegoM.Data.Migrations
{
    public partial class ChangeMerchantsTableToTradersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Merchants_MerchantId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "MerchantId",
                table: "Products",
                newName: "TraderId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_MerchantId",
                table: "Products",
                newName: "IX_Products_TraderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Merchants_TraderId",
                table: "Products",
                column: "TraderId",
                principalTable: "Merchants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Merchants_TraderId",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "TraderId",
                table: "Products",
                newName: "MerchantId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_TraderId",
                table: "Products",
                newName: "IX_Products_MerchantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Merchants_MerchantId",
                table: "Products",
                column: "MerchantId",
                principalTable: "Merchants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
