using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorLoan.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addnotedbookingtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Noted",
                table: "Bookings",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Noted",
                table: "Bookings");
        }
    }
}
