using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LivrariaMHS.Migrations
{
    public partial class AddImagens : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "Livros",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Dados",
                table: "Livros",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nome",
                table: "Livros",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Livros");

            migrationBuilder.DropColumn(
                name: "Dados",
                table: "Livros");

            migrationBuilder.DropColumn(
                name: "Nome",
                table: "Livros");
        }
    }
}
