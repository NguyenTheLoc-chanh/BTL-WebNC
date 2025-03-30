using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BTL_WEBNC.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Size_Id",
                table: "CartDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CartDetails_Size_Id",
                table: "CartDetails",
                column: "Size_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CartDetails_Sizes_Size_Id",
                table: "CartDetails",
                column: "Size_Id",
                principalTable: "Sizes",
                principalColumn: "Size_Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartDetails_Sizes_Size_Id",
                table: "CartDetails");

            migrationBuilder.DropIndex(
                name: "IX_CartDetails_Size_Id",
                table: "CartDetails");

            migrationBuilder.DropColumn(
                name: "Size_Id",
                table: "CartDetails");
        }
    }
}
