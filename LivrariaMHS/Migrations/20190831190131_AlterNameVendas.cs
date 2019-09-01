using Microsoft.EntityFrameworkCore.Migrations;

namespace LivrariaMHS.Migrations
{
    public partial class AlterNameVendas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Venda_Clientes_ClienteID",
                table: "Venda");

            migrationBuilder.DropForeignKey(
                name: "FK_Venda_Livros_LivroID",
                table: "Venda");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Venda",
                table: "Venda");

            migrationBuilder.RenameTable(
                name: "Venda",
                newName: "Vendas");

            migrationBuilder.RenameIndex(
                name: "IX_Venda_LivroID",
                table: "Vendas",
                newName: "IX_Vendas_LivroID");

            migrationBuilder.RenameIndex(
                name: "IX_Venda_ClienteID",
                table: "Vendas",
                newName: "IX_Vendas_ClienteID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vendas",
                table: "Vendas",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Vendas_Clientes_ClienteID",
                table: "Vendas",
                column: "ClienteID",
                principalTable: "Clientes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vendas_Livros_LivroID",
                table: "Vendas",
                column: "LivroID",
                principalTable: "Livros",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vendas_Clientes_ClienteID",
                table: "Vendas");

            migrationBuilder.DropForeignKey(
                name: "FK_Vendas_Livros_LivroID",
                table: "Vendas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vendas",
                table: "Vendas");

            migrationBuilder.RenameTable(
                name: "Vendas",
                newName: "Venda");

            migrationBuilder.RenameIndex(
                name: "IX_Vendas_LivroID",
                table: "Venda",
                newName: "IX_Venda_LivroID");

            migrationBuilder.RenameIndex(
                name: "IX_Vendas_ClienteID",
                table: "Venda",
                newName: "IX_Venda_ClienteID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Venda",
                table: "Venda",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Venda_Clientes_ClienteID",
                table: "Venda",
                column: "ClienteID",
                principalTable: "Clientes",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Venda_Livros_LivroID",
                table: "Venda",
                column: "LivroID",
                principalTable: "Livros",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
