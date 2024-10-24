using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace partymanager.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTipoEventoItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tipoeventoitem",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    idTipoEvento = table.Column<int>(type: "int", nullable: false),
                    idItem = table.Column<int>(type: "int", nullable: false),
                    quantidadePorPessoa = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tipoeventoitem", x => x.id);
                    table.ForeignKey(
                        name: "FK_tipoeventoitem_item_idItem",
                        column: x => x.idItem,
                        principalTable: "item",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_tipoeventoitem_tipoevento_idTipoEvento",
                        column: x => x.idTipoEvento,
                        principalTable: "tipoevento",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tipoeventoitem_idItem",
                table: "tipoeventoitem",
                column: "idItem");

            migrationBuilder.CreateIndex(
                name: "IX_tipoeventoitem_idTipoEvento",
                table: "tipoeventoitem",
                column: "idTipoEvento");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tipoeventoitem");
        }
    }
}
