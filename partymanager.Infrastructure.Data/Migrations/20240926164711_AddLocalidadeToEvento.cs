using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace partymanager.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddLocalidadeToEvento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tipoeventoitem_tipoevento_idTipoEvento",
                table: "tipoeventoitem");

            migrationBuilder.CreateTable(
                name: "usuario",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    senha = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuario", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "evento",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    localidade = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    idTipoEvento = table.Column<int>(type: "int", nullable: false),
                    numeroAdultos = table.Column<int>(type: "int", nullable: false),
                    numeroCriancas = table.Column<int>(type: "int", nullable: false),
                    aluguelEspaco = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    idUsuario = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_evento", x => x.id);
                    table.ForeignKey(
                        name: "FK_evento_tipoevento_idTipoEvento",
                        column: x => x.idTipoEvento,
                        principalTable: "tipoevento",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_evento_usuario_idUsuario",
                        column: x => x.idUsuario,
                        principalTable: "usuario",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "itemcalculado",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idEvento = table.Column<int>(type: "int", nullable: false),
                    idItem = table.Column<int>(type: "int", nullable: false),
                    quantidade = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    precoAproximado = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_itemcalculado", x => x.id);
                    table.ForeignKey(
                        name: "FK_itemcalculado_evento_idEvento",
                        column: x => x.idEvento,
                        principalTable: "evento",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_itemcalculado_item_idItem",
                        column: x => x.idItem,
                        principalTable: "item",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_evento_idTipoEvento",
                table: "evento",
                column: "idTipoEvento");

            migrationBuilder.CreateIndex(
                name: "IX_evento_idUsuario",
                table: "evento",
                column: "idUsuario");

            migrationBuilder.CreateIndex(
                name: "IX_itemcalculado_idEvento",
                table: "itemcalculado",
                column: "idEvento");

            migrationBuilder.CreateIndex(
                name: "IX_itemcalculado_idItem",
                table: "itemcalculado",
                column: "idItem");

            migrationBuilder.AddForeignKey(
                name: "FK_tipoeventoitem_tipoevento_idTipoEvento",
                table: "tipoeventoitem",
                column: "idTipoEvento",
                principalTable: "tipoevento",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_tipoeventoitem_tipoevento_idTipoEvento",
                table: "tipoeventoitem");

            migrationBuilder.DropTable(
                name: "itemcalculado");

            migrationBuilder.DropTable(
                name: "evento");

            migrationBuilder.DropTable(
                name: "usuario");

            migrationBuilder.AddForeignKey(
                name: "FK_tipoeventoitem_tipoevento_idTipoEvento",
                table: "tipoeventoitem",
                column: "idTipoEvento",
                principalTable: "tipoevento",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
