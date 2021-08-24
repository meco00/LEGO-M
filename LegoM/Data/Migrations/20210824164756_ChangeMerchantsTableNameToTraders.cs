namespace LegoM.Data.Migrations
{
  using Microsoft.EntityFrameworkCore.Migrations;

    public partial class ChangeMerchantsTableNameToTraders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Merchants_AspNetUsers_UserId",
                table: "Merchants");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Merchants_TraderId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Merchants",
                table: "Merchants");

            migrationBuilder.RenameTable(
                name: "Merchants",
                newName: "Traders");

            migrationBuilder.RenameIndex(
                name: "IX_Merchants_UserId",
                table: "Traders",
                newName: "IX_Traders_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Traders",
                table: "Traders",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Traders_TraderId",
                table: "Products",
                column: "TraderId",
                principalTable: "Traders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Traders_AspNetUsers_UserId",
                table: "Traders",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Traders_TraderId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Traders_AspNetUsers_UserId",
                table: "Traders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Traders",
                table: "Traders");

            migrationBuilder.RenameTable(
                name: "Traders",
                newName: "Merchants");

            migrationBuilder.RenameIndex(
                name: "IX_Traders_UserId",
                table: "Merchants",
                newName: "IX_Merchants_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Merchants",
                table: "Merchants",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Merchants_AspNetUsers_UserId",
                table: "Merchants",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Merchants_TraderId",
                table: "Products",
                column: "TraderId",
                principalTable: "Merchants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
