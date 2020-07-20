using Microsoft.EntityFrameworkCore.Migrations;

namespace Anime_Studio.DataAccess.Migrations
{
    public partial class UpdateToMangaKeyDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Mangas",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Mangas",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "VolumeNumber",
                table: "Mangas",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Mangas_CategoryId",
                table: "Mangas",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Mangas_Categories_CategoryId",
                table: "Mangas",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Mangas_Categories_CategoryId",
                table: "Mangas");

            migrationBuilder.DropIndex(
                name: "IX_Mangas_CategoryId",
                table: "Mangas");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Mangas");

            migrationBuilder.DropColumn(
                name: "VolumeNumber",
                table: "Mangas");

            migrationBuilder.AlterColumn<string>(
                name: "Price",
                table: "Mangas",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(double));
        }
    }
}
