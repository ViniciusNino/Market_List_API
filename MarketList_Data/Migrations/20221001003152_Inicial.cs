using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MarketList_Data.Migrations
{
    public partial class Inicial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Areas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SDescricao = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Areas", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PerfilUsuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SDescricao = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PerfilUsuario", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sessao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SNome = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Unidade",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SNome = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unidade", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioUnidade",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NIdUsuario = table.Column<int>(type: "integer", nullable: false),
                    NIdUnidade = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioUnidade", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NIdArea = table.Column<int>(type: "integer", nullable: false),
                    SDescricao = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Status_Areas_NIdArea",
                        column: x => x.NIdArea,
                        principalTable: "Areas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SNome = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    NIdSessao = table.Column<int>(type: "integer", nullable: false),
                    NIdUnidade = table.Column<int>(type: "integer", nullable: false),
                    SUnidadeMedida = table.Column<string>(type: "character varying(2)", maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Item_Sessao_NIdSessao",
                        column: x => x.NIdSessao,
                        principalTable: "Sessao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Item_Unidade_NIdUnidade",
                        column: x => x.NIdUnidade,
                        principalTable: "Unidade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SUsuario = table.Column<string>(type: "text", nullable: true),
                    SSenha = table.Column<string>(type: "text", nullable: true),
                    NIdPerfilUsuario = table.Column<int>(type: "integer", nullable: false),
                    NIdStatus = table.Column<int>(type: "integer", nullable: false),
                    DCadastro = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuario_PerfilUsuario_NIdPerfilUsuario",
                        column: x => x.NIdPerfilUsuario,
                        principalTable: "PerfilUsuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Usuario_Status_NIdStatus",
                        column: x => x.NIdStatus,
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AgrupadorListas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NIdUsuario = table.Column<int>(type: "integer", nullable: false),
                    NIdStatus = table.Column<int>(type: "integer", nullable: false),
                    DCadastro = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AgrupadorListas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AgrupadorListas_Status_NIdStatus",
                        column: x => x.NIdStatus,
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AgrupadorListas_Usuario_NIdUsuario",
                        column: x => x.NIdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lista",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NIdUnidade = table.Column<int>(type: "integer", nullable: false),
                    NIdUsuario = table.Column<int>(type: "integer", nullable: false),
                    BAtivo = table.Column<bool>(type: "boolean", nullable: false),
                    DCadastro = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    SNome = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lista", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lista_Unidade_NIdUnidade",
                        column: x => x.NIdUnidade,
                        principalTable: "Unidade",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Lista_Usuario_NIdUsuario",
                        column: x => x.NIdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ItemLista",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NIdLista = table.Column<int>(type: "integer", nullable: false),
                    NIdItem = table.Column<int>(type: "integer", nullable: false),
                    NQuantidade = table.Column<decimal>(type: "numeric", nullable: false),
                    DCadastro = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    BAtivo = table.Column<bool>(type: "boolean", nullable: false),
                    NIdStatus = table.Column<int>(type: "integer", nullable: false),
                    NIdUsuarioSolicitante = table.Column<int>(type: "integer", nullable: false),
                    NIdUsuarioComprador = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemLista", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemLista_Item_NIdItem",
                        column: x => x.NIdItem,
                        principalTable: "Item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemLista_Lista_NIdLista",
                        column: x => x.NIdLista,
                        principalTable: "Lista",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemLista_Status_NIdStatus",
                        column: x => x.NIdStatus,
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ItemLista_Usuario_NIdUsuarioComprador",
                        column: x => x.NIdUsuarioComprador,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ItemLista_Usuario_NIdUsuarioSolicitante",
                        column: x => x.NIdUsuarioSolicitante,
                        principalTable: "Usuario",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ListaAgrupadorListas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NIdAgrupadorListas = table.Column<int>(type: "integer", nullable: false),
                    NIdLista = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListaAgrupadorListas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListaAgrupadorListas_AgrupadorListas_NIdAgrupadorListas",
                        column: x => x.NIdAgrupadorListas,
                        principalTable: "AgrupadorListas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ListaAgrupadorListas_Lista_NIdLista",
                        column: x => x.NIdLista,
                        principalTable: "Lista",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AgrupadorListas_NIdStatus",
                table: "AgrupadorListas",
                column: "NIdStatus");

            migrationBuilder.CreateIndex(
                name: "IX_AgrupadorListas_NIdUsuario",
                table: "AgrupadorListas",
                column: "NIdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Item_NIdSessao",
                table: "Item",
                column: "NIdSessao");

            migrationBuilder.CreateIndex(
                name: "IX_Item_NIdUnidade",
                table: "Item",
                column: "NIdUnidade");

            migrationBuilder.CreateIndex(
                name: "IX_ItemLista_NIdItem",
                table: "ItemLista",
                column: "NIdItem");

            migrationBuilder.CreateIndex(
                name: "IX_ItemLista_NIdLista",
                table: "ItemLista",
                column: "NIdLista");

            migrationBuilder.CreateIndex(
                name: "IX_ItemLista_NIdStatus",
                table: "ItemLista",
                column: "NIdStatus");

            migrationBuilder.CreateIndex(
                name: "IX_ItemLista_NIdUsuarioComprador",
                table: "ItemLista",
                column: "NIdUsuarioComprador");

            migrationBuilder.CreateIndex(
                name: "IX_ItemLista_NIdUsuarioSolicitante",
                table: "ItemLista",
                column: "NIdUsuarioSolicitante");

            migrationBuilder.CreateIndex(
                name: "IX_Lista_NIdUnidade",
                table: "Lista",
                column: "NIdUnidade");

            migrationBuilder.CreateIndex(
                name: "IX_Lista_NIdUsuario",
                table: "Lista",
                column: "NIdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_ListaAgrupadorListas_NIdAgrupadorListas",
                table: "ListaAgrupadorListas",
                column: "NIdAgrupadorListas");

            migrationBuilder.CreateIndex(
                name: "IX_ListaAgrupadorListas_NIdLista",
                table: "ListaAgrupadorListas",
                column: "NIdLista");

            migrationBuilder.CreateIndex(
                name: "IX_Status_NIdArea",
                table: "Status",
                column: "NIdArea");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_NIdPerfilUsuario",
                table: "Usuario",
                column: "NIdPerfilUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_NIdStatus",
                table: "Usuario",
                column: "NIdStatus");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemLista");

            migrationBuilder.DropTable(
                name: "ListaAgrupadorListas");

            migrationBuilder.DropTable(
                name: "UsuarioUnidade");

            migrationBuilder.DropTable(
                name: "Item");

            migrationBuilder.DropTable(
                name: "AgrupadorListas");

            migrationBuilder.DropTable(
                name: "Lista");

            migrationBuilder.DropTable(
                name: "Sessao");

            migrationBuilder.DropTable(
                name: "Unidade");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "PerfilUsuario");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropTable(
                name: "Areas");
        }
    }
}
