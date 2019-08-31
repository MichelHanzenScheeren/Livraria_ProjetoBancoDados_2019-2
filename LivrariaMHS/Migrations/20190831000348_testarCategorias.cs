using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LivrariaMHS.Migrations
{
    public partial class testarCategorias : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoriaID",
                table: "Livros",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LivroCategoria",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    LivroID = table.Column<int>(nullable: false),
                    CategoriaID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LivroCategoria", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LivroCategoria_Categorias_CategoriaID",
                        column: x => x.CategoriaID,
                        principalTable: "Categorias",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LivroCategoria_Livros_LivroID",
                        column: x => x.LivroID,
                        principalTable: "Livros",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Livros_CategoriaID",
                table: "Livros",
                column: "CategoriaID");

            migrationBuilder.CreateIndex(
                name: "IX_LivroCategoria_CategoriaID",
                table: "LivroCategoria",
                column: "CategoriaID");

            migrationBuilder.CreateIndex(
                name: "IX_LivroCategoria_LivroID",
                table: "LivroCategoria",
                column: "LivroID");

            migrationBuilder.AddForeignKey(
                name: "FK_Livros_Categorias_CategoriaID",
                table: "Livros",
                column: "CategoriaID",
                principalTable: "Categorias",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Livros_Categorias_CategoriaID",
                table: "Livros");

            migrationBuilder.DropTable(
                name: "LivroCategoria");

            migrationBuilder.DropIndex(
                name: "IX_Livros_CategoriaID",
                table: "Livros");

            migrationBuilder.DropColumn(
                name: "CategoriaID",
                table: "Livros");
        }
    }
}
