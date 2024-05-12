using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MarketList_Data.Migrations
{
    /// <inheritdoc />
    public partial class primeiraMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Area",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SDescricao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ClassLevelCascadeMode = table.Column<int>(type: "int", nullable: false),
                    RuleLevelCascadeMode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Area", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Sessao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SNome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ClassLevelCascadeMode = table.Column<int>(type: "int", nullable: false),
                    RuleLevelCascadeMode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessao", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Unidade",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SNome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ClassLevelCascadeMode = table.Column<int>(type: "int", nullable: false),
                    RuleLevelCascadeMode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unidade", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NIdArea = table.Column<int>(type: "int", nullable: false),
                    SDescricao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ClassLevelCascadeMode = table.Column<int>(type: "int", nullable: false),
                    RuleLevelCascadeMode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Status_Area_NIdArea",
                        column: x => x.NIdArea,
                        principalTable: "Area",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Tipo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NIdArea = table.Column<int>(type: "int", nullable: false),
                    SDescricao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ClassLevelCascadeMode = table.Column<int>(type: "int", nullable: false),
                    RuleLevelCascadeMode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tipo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tipo_Area_NIdArea",
                        column: x => x.NIdArea,
                        principalTable: "Area",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SNome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SUnidadeMedida = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    NIdSessao = table.Column<int>(type: "int", nullable: false),
                    NIdUnidade = table.Column<int>(type: "int", nullable: false),
                    ClassLevelCascadeMode = table.Column<int>(type: "int", nullable: false),
                    RuleLevelCascadeMode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Item_Sessao_NIdSessao",
                        column: x => x.NIdSessao,
                        principalTable: "Sessao",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Item_Unidade_NIdUnidade",
                        column: x => x.NIdUnidade,
                        principalTable: "Unidade",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SUsuario = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SNome = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    SSenha = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    SEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    NIdTipo = table.Column<int>(type: "int", nullable: false),
                    NIdStatus = table.Column<int>(type: "int", nullable: false),
                    DCadastro = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    ClassLevelCascadeMode = table.Column<int>(type: "int", nullable: false),
                    RuleLevelCascadeMode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Usuario_Status_NIdStatus",
                        column: x => x.NIdStatus,
                        principalTable: "Status",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Usuario_Tipo_NIdTipo",
                        column: x => x.NIdTipo,
                        principalTable: "Tipo",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Agrupador",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NIdUsuario = table.Column<int>(type: "int", nullable: false),
                    NIdStatus = table.Column<int>(type: "int", nullable: false, defaultValue: 5),
                    DCadastro = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    SDescricao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ClassLevelCascadeMode = table.Column<int>(type: "int", nullable: false),
                    RuleLevelCascadeMode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Agrupador", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Agrupador_Status_NIdStatus",
                        column: x => x.NIdStatus,
                        principalTable: "Status",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Agrupador_Usuario_NIdUsuario",
                        column: x => x.NIdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Lista",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NIdUnidade = table.Column<int>(type: "int", nullable: false),
                    NIdUsuario = table.Column<int>(type: "int", nullable: false),
                    BAtivo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    DCadastro = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    SNome = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClassLevelCascadeMode = table.Column<int>(type: "int", nullable: false),
                    RuleLevelCascadeMode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lista", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Lista_Unidade_NIdUnidade",
                        column: x => x.NIdUnidade,
                        principalTable: "Unidade",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Lista_Usuario_NIdUsuario",
                        column: x => x.NIdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UsuarioUnidade",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NIdUsuario = table.Column<int>(type: "int", nullable: false),
                    NIdUnidade = table.Column<int>(type: "int", nullable: false),
                    ClassLevelCascadeMode = table.Column<int>(type: "int", nullable: false),
                    RuleLevelCascadeMode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioUnidade", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsuarioUnidade_Unidade_NIdUnidade",
                        column: x => x.NIdUnidade,
                        principalTable: "Unidade",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UsuarioUnidade_Usuario_NIdUsuario",
                        column: x => x.NIdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VerificacaoToken",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NIdUsuario = table.Column<int>(type: "int", nullable: false),
                    NIdTipo = table.Column<int>(type: "int", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DCadastro = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    BAtivo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    ClassLevelCascadeMode = table.Column<int>(type: "int", nullable: false),
                    RuleLevelCascadeMode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerificacaoToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VerificacaoToken_Tipo_NIdTipo",
                        column: x => x.NIdTipo,
                        principalTable: "Tipo",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VerificacaoToken_Usuario_NIdUsuario",
                        column: x => x.NIdUsuario,
                        principalTable: "Usuario",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ItemLista",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NIdLista = table.Column<int>(type: "int", nullable: false),
                    NIdItem = table.Column<int>(type: "int", nullable: false),
                    NQuantidade = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DCadastro = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    BAtivo = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    NIdStatus = table.Column<int>(type: "int", nullable: false, defaultValue: 2),
                    NIdUsuarioSolicitante = table.Column<int>(type: "int", nullable: false),
                    NIdUsuarioComprador = table.Column<int>(type: "int", nullable: false),
                    ClassLevelCascadeMode = table.Column<int>(type: "int", nullable: false),
                    RuleLevelCascadeMode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ItemLista", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ItemLista_Item_NIdItem",
                        column: x => x.NIdItem,
                        principalTable: "Item",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ItemLista_Lista_NIdLista",
                        column: x => x.NIdLista,
                        principalTable: "Lista",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ItemLista_Status_NIdStatus",
                        column: x => x.NIdStatus,
                        principalTable: "Status",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ItemLista_Usuario_NIdUsuarioComprador",
                        column: x => x.NIdUsuarioComprador,
                        principalTable: "Usuario",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ItemLista_Usuario_NIdUsuarioSolicitante",
                        column: x => x.NIdUsuarioSolicitante,
                        principalTable: "Usuario",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ListaAgrupador",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NIdAgrupadorListas = table.Column<int>(type: "int", nullable: false),
                    NIdLista = table.Column<int>(type: "int", nullable: false),
                    ClassLevelCascadeMode = table.Column<int>(type: "int", nullable: false),
                    RuleLevelCascadeMode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ListaAgrupador", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ListaAgrupador_Agrupador_NIdAgrupadorListas",
                        column: x => x.NIdAgrupadorListas,
                        principalTable: "Agrupador",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ListaAgrupador_Lista_NIdLista",
                        column: x => x.NIdLista,
                        principalTable: "Lista",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Agrupador_NIdStatus",
                table: "Agrupador",
                column: "NIdStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Agrupador_NIdUsuario",
                table: "Agrupador",
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
                name: "IX_ListaAgrupador_NIdAgrupadorListas",
                table: "ListaAgrupador",
                column: "NIdAgrupadorListas");

            migrationBuilder.CreateIndex(
                name: "IX_ListaAgrupador_NIdLista",
                table: "ListaAgrupador",
                column: "NIdLista");

            migrationBuilder.CreateIndex(
                name: "IX_Status_NIdArea",
                table: "Status",
                column: "NIdArea");

            migrationBuilder.CreateIndex(
                name: "IX_Tipo_NIdArea",
                table: "Tipo",
                column: "NIdArea");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_NIdStatus",
                table: "Usuario",
                column: "NIdStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_NIdTipo",
                table: "Usuario",
                column: "NIdTipo");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_SEmail",
                table: "Usuario",
                column: "SEmail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioUnidade_NIdUnidade",
                table: "UsuarioUnidade",
                column: "NIdUnidade");

            migrationBuilder.CreateIndex(
                name: "IX_UsuarioUnidade_NIdUsuario",
                table: "UsuarioUnidade",
                column: "NIdUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_VerificacaoToken_NIdTipo",
                table: "VerificacaoToken",
                column: "NIdTipo");

            migrationBuilder.CreateIndex(
                name: "IX_VerificacaoToken_NIdUsuario",
                table: "VerificacaoToken",
                column: "NIdUsuario");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ItemLista");

            migrationBuilder.DropTable(
                name: "ListaAgrupador");

            migrationBuilder.DropTable(
                name: "UsuarioUnidade");

            migrationBuilder.DropTable(
                name: "VerificacaoToken");

            migrationBuilder.DropTable(
                name: "Item");

            migrationBuilder.DropTable(
                name: "Agrupador");

            migrationBuilder.DropTable(
                name: "Lista");

            migrationBuilder.DropTable(
                name: "Sessao");

            migrationBuilder.DropTable(
                name: "Unidade");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropTable(
                name: "Tipo");

            migrationBuilder.DropTable(
                name: "Area");
        }
    }
}
