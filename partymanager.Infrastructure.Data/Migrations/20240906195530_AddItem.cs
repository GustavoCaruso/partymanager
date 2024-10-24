using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace partymanager.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Adicionar a coluna com um valor padrão válido
            migrationBuilder.AddColumn<int>(
                name: "idTipoItem",
                table: "item",
                type: "int",
                nullable: false,
                defaultValue: 1);  // Certifique-se de que o ID 1 existe na tabela tipoitem

            migrationBuilder.CreateIndex(
                name: "IX_item_idTipoItem",
                table: "item",
                column: "idTipoItem");

            migrationBuilder.AddForeignKey(
                name: "FK_item_tipoitem_idTipoItem",
                table: "item",
                column: "idTipoItem",
                principalTable: "tipoitem",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_item_tipoitem_idTipoItem",
                table: "item");

            migrationBuilder.DropIndex(
                name: "IX_item_idTipoItem",
                table: "item");

            migrationBuilder.DropColumn(
                name: "idTipoItem",
                table: "item");
        }
    }
}
