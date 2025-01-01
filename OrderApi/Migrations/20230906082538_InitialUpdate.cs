using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OrderApi.Migrations
{
    public partial class InitialUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductEntity_Orders_OrderEntityOrderId",
                table: "ProductEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductEntity",
                table: "ProductEntity");

            migrationBuilder.DropIndex(
                name: "IX_ProductEntity_OrderEntityOrderId",
                table: "ProductEntity");

            migrationBuilder.DropColumn(
                name: "OrderEntityOrderId",
                table: "ProductEntity");

            migrationBuilder.RenameTable(
                name: "ProductEntity",
                newName: "Products");

            migrationBuilder.AddColumn<int>(
                name: "OrderId",
                table: "Products",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Products_OrderId",
                table: "Products",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Orders_OrderId",
                table: "Products",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_Orders_OrderId",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_OrderId",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "ProductEntity");

            migrationBuilder.AddColumn<int>(
                name: "OrderEntityOrderId",
                table: "ProductEntity",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductEntity",
                table: "ProductEntity",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProductEntity_OrderEntityOrderId",
                table: "ProductEntity",
                column: "OrderEntityOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductEntity_Orders_OrderEntityOrderId",
                table: "ProductEntity",
                column: "OrderEntityOrderId",
                principalTable: "Orders",
                principalColumn: "OrderId");
        }
    }
}
