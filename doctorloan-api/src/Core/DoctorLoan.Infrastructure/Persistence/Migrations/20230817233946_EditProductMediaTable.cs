using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorLoan.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EditProductMediaTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductItemId",
                table: "ProductMedias",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductItemId",
                table: "ProductMedias");
        }
    }
}
