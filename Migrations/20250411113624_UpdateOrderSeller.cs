using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BTL_WEBNC.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderSeller : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "seller_Id",
                table: "Orders",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_seller_Id",
                table: "Orders",
                column: "seller_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Sellers_seller_Id",
                table: "Orders",
                column: "seller_Id",
                principalTable: "Sellers",
                principalColumn: "seller_Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Sellers_seller_Id",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_seller_Id",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "seller_Id",
                table: "Orders");
        }
    }
}
