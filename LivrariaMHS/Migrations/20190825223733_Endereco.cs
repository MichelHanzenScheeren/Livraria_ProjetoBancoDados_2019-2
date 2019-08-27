using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LivrariaMHS.Migrations
{
    public partial class Endereco : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BairroID",
                table: "Cliente",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CidadeID",
                table: "Cliente",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Numero",
                table: "Cliente",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "RuaID",
                table: "Cliente",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Bairro",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(maxLength: 70, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bairro", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Cidade",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(maxLength: 70, nullable: false),
                    Estado = table.Column<string>(maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cidade", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Rua",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(maxLength: 70, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rua", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_BairroID",
                table: "Cliente",
                column: "BairroID");

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_CidadeID",
                table: "Cliente",
                column: "CidadeID");

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_RuaID",
                table: "Cliente",
                column: "RuaID");

            migrationBuilder.AddForeignKey(
                name: "FK_Cliente_Bairro_BairroID",
                table: "Cliente",
                column: "BairroID",
                principalTable: "Bairro",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cliente_Cidade_CidadeID",
                table: "Cliente",
                column: "CidadeID",
                principalTable: "Cidade",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cliente_Rua_RuaID",
                table: "Cliente",
                column: "RuaID",
                principalTable: "Rua",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cliente_Bairro_BairroID",
                table: "Cliente");

            migrationBuilder.DropForeignKey(
                name: "FK_Cliente_Cidade_CidadeID",
                table: "Cliente");

            migrationBuilder.DropForeignKey(
                name: "FK_Cliente_Rua_RuaID",
                table: "Cliente");

            migrationBuilder.DropTable(
                name: "Bairro");

            migrationBuilder.DropTable(
                name: "Cidade");

            migrationBuilder.DropTable(
                name: "Rua");

            migrationBuilder.DropIndex(
                name: "IX_Cliente_BairroID",
                table: "Cliente");

            migrationBuilder.DropIndex(
                name: "IX_Cliente_CidadeID",
                table: "Cliente");

            migrationBuilder.DropIndex(
                name: "IX_Cliente_RuaID",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "BairroID",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "CidadeID",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "Numero",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "RuaID",
                table: "Cliente");
        }
    }
}
