using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketList_Data.Migrations
{
    /// <inheritdoc />
    public partial class AtualizarUnidade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "BSelecionado",
                table: "UsuarioUnidade",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "NIdUnidade",
                table: "Usuario",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DCadastro",
                table: "Unidade",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_NIdUnidade",
                table: "Usuario",
                column: "NIdUnidade");

            migrationBuilder.AddForeignKey(
                name: "FK_Usuario_Unidade_NIdUnidade",
                table: "Usuario",
                column: "NIdUnidade",
                principalTable: "Unidade",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Usuario_Unidade_NIdUnidade",
                table: "Usuario");

            migrationBuilder.DropIndex(
                name: "IX_Usuario_NIdUnidade",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "BSelecionado",
                table: "UsuarioUnidade");

            migrationBuilder.DropColumn(
                name: "NIdUnidade",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "DCadastro",
                table: "Unidade");
        }
    }
}
