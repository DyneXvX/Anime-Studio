using Microsoft.EntityFrameworkCore.Migrations;

namespace Anime_Studio.DataAccess.Migrations
{
    public partial class AddMangaToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mangas",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: false),
                    Author = table.Column<string>(nullable: false),
                    Artist = table.Column<string>(nullable: false),
                    PublishingCompany = table.Column<string>(nullable: true),
                    ISBN = table.Column<string>(maxLength: 20, nullable: false),
                    Price = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mangas", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Mangas");
        }
    }
}
