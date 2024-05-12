using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketList_Data.Migrations
{
    /// <inheritdoc />
    public partial class SegundaMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClassLevelCascadeMode",
                table: "VerificacaoToken");

            migrationBuilder.DropColumn(
                name: "ClassLevelCascadeMode",
                table: "UsuarioUnidade");

            migrationBuilder.DropColumn(
                name: "ClassLevelCascadeMode",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "ClassLevelCascadeMode",
                table: "Unidade");

            migrationBuilder.DropColumn(
                name: "ClassLevelCascadeMode",
                table: "Tipo");

            migrationBuilder.DropColumn(
                name: "ClassLevelCascadeMode",
                table: "Status");

            migrationBuilder.DropColumn(
                name: "ClassLevelCascadeMode",
                table: "Sessao");

            migrationBuilder.DropColumn(
                name: "ClassLevelCascadeMode",
                table: "ListaAgrupador");

            migrationBuilder.DropColumn(
                name: "ClassLevelCascadeMode",
                table: "Lista");

            migrationBuilder.DropColumn(
                name: "ClassLevelCascadeMode",
                table: "ItemLista");

            migrationBuilder.DropColumn(
                name: "ClassLevelCascadeMode",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "ClassLevelCascadeMode",
                table: "Area");

            migrationBuilder.DropColumn(
                name: "ClassLevelCascadeMode",
                table: "Agrupador");

            migrationBuilder.RenameColumn(
                name: "RuleLevelCascadeMode",
                table: "VerificacaoToken",
                newName: "CascadeMode");

            migrationBuilder.RenameColumn(
                name: "RuleLevelCascadeMode",
                table: "UsuarioUnidade",
                newName: "CascadeMode");

            migrationBuilder.RenameColumn(
                name: "RuleLevelCascadeMode",
                table: "Usuario",
                newName: "CascadeMode");

            migrationBuilder.RenameColumn(
                name: "RuleLevelCascadeMode",
                table: "Unidade",
                newName: "CascadeMode");

            migrationBuilder.RenameColumn(
                name: "RuleLevelCascadeMode",
                table: "Tipo",
                newName: "CascadeMode");

            migrationBuilder.RenameColumn(
                name: "RuleLevelCascadeMode",
                table: "Status",
                newName: "CascadeMode");

            migrationBuilder.RenameColumn(
                name: "RuleLevelCascadeMode",
                table: "Sessao",
                newName: "CascadeMode");

            migrationBuilder.RenameColumn(
                name: "RuleLevelCascadeMode",
                table: "ListaAgrupador",
                newName: "CascadeMode");

            migrationBuilder.RenameColumn(
                name: "RuleLevelCascadeMode",
                table: "Lista",
                newName: "CascadeMode");

            migrationBuilder.RenameColumn(
                name: "RuleLevelCascadeMode",
                table: "ItemLista",
                newName: "CascadeMode");

            migrationBuilder.RenameColumn(
                name: "RuleLevelCascadeMode",
                table: "Item",
                newName: "CascadeMode");

            migrationBuilder.RenameColumn(
                name: "RuleLevelCascadeMode",
                table: "Area",
                newName: "CascadeMode");

            migrationBuilder.RenameColumn(
                name: "RuleLevelCascadeMode",
                table: "Agrupador",
                newName: "CascadeMode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CascadeMode",
                table: "VerificacaoToken",
                newName: "RuleLevelCascadeMode");

            migrationBuilder.RenameColumn(
                name: "CascadeMode",
                table: "UsuarioUnidade",
                newName: "RuleLevelCascadeMode");

            migrationBuilder.RenameColumn(
                name: "CascadeMode",
                table: "Usuario",
                newName: "RuleLevelCascadeMode");

            migrationBuilder.RenameColumn(
                name: "CascadeMode",
                table: "Unidade",
                newName: "RuleLevelCascadeMode");

            migrationBuilder.RenameColumn(
                name: "CascadeMode",
                table: "Tipo",
                newName: "RuleLevelCascadeMode");

            migrationBuilder.RenameColumn(
                name: "CascadeMode",
                table: "Status",
                newName: "RuleLevelCascadeMode");

            migrationBuilder.RenameColumn(
                name: "CascadeMode",
                table: "Sessao",
                newName: "RuleLevelCascadeMode");

            migrationBuilder.RenameColumn(
                name: "CascadeMode",
                table: "ListaAgrupador",
                newName: "RuleLevelCascadeMode");

            migrationBuilder.RenameColumn(
                name: "CascadeMode",
                table: "Lista",
                newName: "RuleLevelCascadeMode");

            migrationBuilder.RenameColumn(
                name: "CascadeMode",
                table: "ItemLista",
                newName: "RuleLevelCascadeMode");

            migrationBuilder.RenameColumn(
                name: "CascadeMode",
                table: "Item",
                newName: "RuleLevelCascadeMode");

            migrationBuilder.RenameColumn(
                name: "CascadeMode",
                table: "Area",
                newName: "RuleLevelCascadeMode");

            migrationBuilder.RenameColumn(
                name: "CascadeMode",
                table: "Agrupador",
                newName: "RuleLevelCascadeMode");

            migrationBuilder.AddColumn<int>(
                name: "ClassLevelCascadeMode",
                table: "VerificacaoToken",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ClassLevelCascadeMode",
                table: "UsuarioUnidade",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ClassLevelCascadeMode",
                table: "Usuario",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ClassLevelCascadeMode",
                table: "Unidade",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ClassLevelCascadeMode",
                table: "Tipo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ClassLevelCascadeMode",
                table: "Status",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ClassLevelCascadeMode",
                table: "Sessao",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ClassLevelCascadeMode",
                table: "ListaAgrupador",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ClassLevelCascadeMode",
                table: "Lista",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ClassLevelCascadeMode",
                table: "ItemLista",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ClassLevelCascadeMode",
                table: "Item",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ClassLevelCascadeMode",
                table: "Area",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ClassLevelCascadeMode",
                table: "Agrupador",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
