using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorLoan.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class deleteoptionIdorderitemtable3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_ProductOptionGroups_ProductOptionGroupId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_ProductOptionGroupId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "ProductOptionGroupId",
                table: "OrderItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductOptionGroupId",
                table: "OrderItems",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductOptionGroupId",
                table: "OrderItems",
                column: "ProductOptionGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_ProductOptionGroups_ProductOptionGroupId",
                table: "OrderItems",
                column: "ProductOptionGroupId",
                principalTable: "ProductOptionGroups",
                principalColumn: "Id");
        }
    }
}
