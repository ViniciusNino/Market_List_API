using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketList_Data.Migrations
{
    /// <inheritdoc />
    public partial class AtualizarUnidade2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BSelecionado",
                table: "UsuarioUnidade");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "BSelecionado",
                table: "UsuarioUnidade",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
