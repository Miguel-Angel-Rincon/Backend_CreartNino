using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api_CreartNino.Migrations
{
    /// <inheritdoc />
    public partial class cascadequenosirvenenlosdetalles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetallePedido_Producto",
                table: "Detalle_Pedido");

            migrationBuilder.DropForeignKey(
                name: "FK_DetalleProduccion_IdInsumo",
                table: "Detalle_Produccion");

            migrationBuilder.DropForeignKey(
                name: "FK_DetalleProduccion_IdProducto",
                table: "Detalle_Produccion");

            migrationBuilder.DropForeignKey(
                name: "FK_Detalle_Produccion_Pedidos_IdPedido",
                table: "Detalle_Produccion");

            migrationBuilder.DropForeignKey(
                name: "FK_DetallesCompra_Insumo",
                table: "Detalles_Compra");

            migrationBuilder.AddForeignKey(
                name: "FK_DetallePedido_Producto",
                table: "Detalle_Pedido",
                column: "IdProducto",
                principalTable: "Productos",
                principalColumn: "IdProducto",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleProduccion_IdInsumo",
                table: "Detalle_Produccion",
                column: "IdInsumo",
                principalTable: "Insumos",
                principalColumn: "IdInsumo",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleProduccion_IdProducto",
                table: "Detalle_Produccion",
                column: "IdProducto",
                principalTable: "Productos",
                principalColumn: "IdProducto",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Detalle_Produccion_Pedidos_IdPedido",
                table: "Detalle_Produccion",
                column: "IdPedido",
                principalTable: "Pedidos",
                principalColumn: "IdPedido",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DetallesCompra_Insumo",
                table: "Detalles_Compra",
                column: "IdInsumo",
                principalTable: "Insumos",
                principalColumn: "IdInsumo",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DetallePedido_Producto",
                table: "Detalle_Pedido");

            migrationBuilder.DropForeignKey(
                name: "FK_DetalleProduccion_IdInsumo",
                table: "Detalle_Produccion");

            migrationBuilder.DropForeignKey(
                name: "FK_DetalleProduccion_IdProducto",
                table: "Detalle_Produccion");

            migrationBuilder.DropForeignKey(
                name: "FK_Detalle_Produccion_Pedidos_IdPedido",
                table: "Detalle_Produccion");

            migrationBuilder.DropForeignKey(
                name: "FK_DetallesCompra_Insumo",
                table: "Detalles_Compra");

            migrationBuilder.AddForeignKey(
                name: "FK_DetallePedido_Producto",
                table: "Detalle_Pedido",
                column: "IdProducto",
                principalTable: "Productos",
                principalColumn: "IdProducto",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleProduccion_IdInsumo",
                table: "Detalle_Produccion",
                column: "IdInsumo",
                principalTable: "Insumos",
                principalColumn: "IdInsumo",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DetalleProduccion_IdProducto",
                table: "Detalle_Produccion",
                column: "IdProducto",
                principalTable: "Productos",
                principalColumn: "IdProducto",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Detalle_Produccion_Pedidos_IdPedido",
                table: "Detalle_Produccion",
                column: "IdPedido",
                principalTable: "Pedidos",
                principalColumn: "IdPedido",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DetallesCompra_Insumo",
                table: "Detalles_Compra",
                column: "IdInsumo",
                principalTable: "Insumos",
                principalColumn: "IdInsumo",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
