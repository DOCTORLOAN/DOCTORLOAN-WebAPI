using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DoctorLoan.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationShip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductMedias_Medias_MeidaId",
                table: "ProductMedias");

            migrationBuilder.DropIndex(
                name: "IX_ProductMedias_MeidaId",
                table: "ProductMedias");

            migrationBuilder.DropColumn(
                name: "MeidaId",
                table: "ProductMedias");

            migrationBuilder.AlterColumn<long>(
                name: "MediaId",
                table: "ProductMedias",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_ProductMedias_MediaId",
                table: "ProductMedias",
                column: "MediaId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMedias_Medias_MediaId",
                table: "ProductMedias",
                column: "MediaId",
                principalTable: "Medias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductMedias_Medias_MediaId",
                table: "ProductMedias");

            migrationBuilder.DropIndex(
                name: "IX_ProductMedias_MediaId",
                table: "ProductMedias");

            migrationBuilder.AlterColumn<int>(
                name: "MediaId",
                table: "ProductMedias",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "MeidaId",
                table: "ProductMedias",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductMedias_MeidaId",
                table: "ProductMedias",
                column: "MeidaId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductMedias_Medias_MeidaId",
                table: "ProductMedias",
                column: "MeidaId",
                principalTable: "Medias",
                principalColumn: "Id");
        }
    }
}
