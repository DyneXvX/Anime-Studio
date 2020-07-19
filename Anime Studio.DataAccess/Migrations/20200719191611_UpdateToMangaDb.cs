using Microsoft.EntityFrameworkCore.Migrations;

namespace Anime_Studio.DataAccess.Migrations
{
    public partial class UpdateToMangaDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cover",
                table: "Mangas",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cover",
                table: "Mangas");
        }
    }
}
