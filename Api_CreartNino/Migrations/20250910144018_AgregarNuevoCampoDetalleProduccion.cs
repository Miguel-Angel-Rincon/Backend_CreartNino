using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api_CreartNino.Migrations
{
    /// <inheritdoc />
    public partial class AgregarNuevoCampoDetalleProduccion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CantidadInsumo",
                table: "Detalle_Produccion",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CantidadInsumo",
                table: "Detalle_Produccion");
        }
    }
}
