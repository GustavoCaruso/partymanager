using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace partymanager.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddModificacoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "precoAproximado",
                table: "itemcalculado");

            migrationBuilder.DropColumn(
                name: "aluguelEspaco",
                table: "evento");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "precoAproximado",
                table: "itemcalculado",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "aluguelEspaco",
                table: "evento",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
