using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LivrariaMHS.Migrations
{
    public partial class Add_Imagem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Imagem",
                table: "Livros",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Imagem",
                table: "Livros");
        }
    }
}
