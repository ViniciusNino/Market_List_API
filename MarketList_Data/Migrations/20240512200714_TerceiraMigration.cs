using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketList_Data.Migrations
{
    /// <inheritdoc />
    public partial class TerceiraMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CascadeMode",
                table: "VerificacaoToken");

            migrationBuilder.DropColumn(
                name: "CascadeMode",
                table: "UsuarioUnidade");

            migrationBuilder.DropColumn(
                name: "CascadeMode",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "CascadeMode",
                table: "Unidade");

            migrationBuilder.DropColumn(
                name: "CascadeMode",
                table: "Tipo");

            migrationBuilder.DropColumn(
                name: "CascadeMode",
                table: "Status");

            migrationBuilder.DropColumn(
                name: "CascadeMode",
                table: "Sessao");

            migrationBuilder.DropColumn(
                name: "CascadeMode",
                table: "ListaAgrupador");

            migrationBuilder.DropColumn(
                name: "CascadeMode",
                table: "Lista");

            migrationBuilder.DropColumn(
                name: "CascadeMode",
                table: "ItemLista");

            migrationBuilder.DropColumn(
                name: "CascadeMode",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "CascadeMode",
                table: "Area");

            migrationBuilder.DropColumn(
                name: "CascadeMode",
                table: "Agrupador");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CascadeMode",
                table: "VerificacaoToken",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CascadeMode",
                table: "UsuarioUnidade",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CascadeMode",
                table: "Usuario",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CascadeMode",
                table: "Unidade",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CascadeMode",
                table: "Tipo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CascadeMode",
                table: "Status",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CascadeMode",
                table: "Sessao",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CascadeMode",
                table: "ListaAgrupador",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CascadeMode",
                table: "Lista",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CascadeMode",
                table: "ItemLista",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CascadeMode",
                table: "Item",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CascadeMode",
                table: "Area",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CascadeMode",
                table: "Agrupador",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
