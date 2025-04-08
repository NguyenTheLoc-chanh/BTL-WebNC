using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BTL_WEBNC.Migrations
{
    /// <inheritdoc />
    public partial class AddPaymentTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    pay_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    payment_method = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    order_Id = table.Column<int>(type: "int", maxLength: 50, nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.pay_id);
                    table.ForeignKey(
                        name: "FK_Payments_Orders_order_Id",
                        column: x => x.order_Id,
                        principalTable: "Orders",
                        principalColumn: "order_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_order_Id",
                table: "Payments",
                column: "order_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments");
        }
    }
}
