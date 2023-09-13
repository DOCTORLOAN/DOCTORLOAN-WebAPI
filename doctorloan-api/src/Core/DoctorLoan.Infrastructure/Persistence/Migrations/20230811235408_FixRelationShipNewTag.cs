using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorLoan.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixRelationShipNewTag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsTagsMappings_NewsItems_NewsTagId",
                table: "NewsTagsMappings");

            migrationBuilder.CreateIndex(
                name: "IX_NewsTagsMappings_NewsItemId",
                table: "NewsTagsMappings",
                column: "NewsItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsTagsMappings_NewsItems_NewsItemId",
                table: "NewsTagsMappings",
                column: "NewsItemId",
                principalTable: "NewsItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NewsTagsMappings_NewsItems_NewsItemId",
                table: "NewsTagsMappings");

            migrationBuilder.DropIndex(
                name: "IX_NewsTagsMappings_NewsItemId",
                table: "NewsTagsMappings");

            migrationBuilder.AddForeignKey(
                name: "FK_NewsTagsMappings_NewsItems_NewsTagId",
                table: "NewsTagsMappings",
                column: "NewsTagId",
                principalTable: "NewsItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
