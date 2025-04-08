using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BTL_WEBNC.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderDetailsNavigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Orders_order_Id",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AddressUser_Id",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Orders_order_Id",
                table: "Payments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_Id",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "order_Id",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Orders",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_address_Id",
                table: "Orders",
                column: "address_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Orders_order_Id",
                table: "OrderDetails",
                column: "order_Id",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AddressUser_address_Id",
                table: "Orders",
                column: "address_Id",
                principalTable: "AddressUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Orders_order_Id",
                table: "Payments",
                column: "order_Id",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Orders_order_Id",
                table: "OrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AddressUser_address_Id",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Orders_order_Id",
                table: "Payments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Orders",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_address_Id",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Orders",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "order_Id",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Orders",
                table: "Orders",
                column: "order_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Id",
                table: "Orders",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Orders_order_Id",
                table: "OrderDetails",
                column: "order_Id",
                principalTable: "Orders",
                principalColumn: "order_Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AddressUser_Id",
                table: "Orders",
                column: "Id",
                principalTable: "AddressUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Orders_order_Id",
                table: "Payments",
                column: "order_Id",
                principalTable: "Orders",
                principalColumn: "order_Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
