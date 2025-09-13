using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api_CreartNino.Migrations
{
    /// <inheritdoc />
    public partial class AddIdPedidoToDetalleProduccion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IdPedido",
                table: "Detalle_Produccion",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Detalle_Produccion_IdPedido",
                table: "Detalle_Produccion",
                column: "IdPedido");

            migrationBuilder.AddForeignKey(
                name: "FK_Detalle_Produccion_Pedidos_IdPedido",
                table: "Detalle_Produccion",
                column: "IdPedido",
                principalTable: "Pedidos",
                principalColumn: "IdPedido",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Detalle_Produccion_Pedidos_IdPedido",
                table: "Detalle_Produccion");

            migrationBuilder.DropIndex(
                name: "IX_Detalle_Produccion_IdPedido",
                table: "Detalle_Produccion");

            migrationBuilder.DropColumn(
                name: "IdPedido",
                table: "Detalle_Produccion");
        }
    }
}
