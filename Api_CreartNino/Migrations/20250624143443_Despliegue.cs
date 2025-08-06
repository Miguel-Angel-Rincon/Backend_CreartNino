using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api_CreartNino.Migrations
{
    /// <inheritdoc />
    public partial class Despliegue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categoria_Insumo",
                columns: table => new
                {
                    IdCatInsumo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCategoria = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Descripcion = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Categori__6815ADDF91908ECC", x => x.IdCatInsumo);
                });

            migrationBuilder.CreateTable(
                name: "Categoria_Productos",
                columns: table => new
                {
                    IdCategoriaProducto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoriaProducto = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Descripcion = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Categori__8A4F21C36F377EF0", x => x.IdCategoriaProducto);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    IdCliente = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCompleto = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    TipoDocumento = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    NumDocumento = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Correo = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Celular = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Departamento = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Ciudad = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Direccion = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Clientes__D5946642051B7E6E", x => x.IdCliente);
                });

            migrationBuilder.CreateTable(
                name: "Estado_Compra",
                columns: table => new
                {
                    IdEstado = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreEstado = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Estado_C__FBB0EDC15C474046", x => x.IdEstado);
                });

            migrationBuilder.CreateTable(
                name: "Estados_Pedido",
                columns: table => new
                {
                    IdEstadoPedidos = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreEstado = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Estados___4243CC3E3ADA014C", x => x.IdEstadoPedidos);
                });

            migrationBuilder.CreateTable(
                name: "Estados_Produccion",
                columns: table => new
                {
                    IdEstadoProduccion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreEstado = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Estados___6DEB350E9C367EDB", x => x.IdEstadoProduccion);
                });

            migrationBuilder.CreateTable(
                name: "Imagenes_Productos",
                columns: table => new
                {
                    IdImagen = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    URL = table.Column<string>(type: "varchar(300)", unicode: false, maxLength: 300, nullable: true),
                    Descripcion = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Imagenes__B42D8F2A203F6994", x => x.IdImagen);
                });

            migrationBuilder.CreateTable(
                name: "Marca",
                columns: table => new
                {
                    IdMarca = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreMarca = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Marca__4076A8873CD5B467", x => x.IdMarca);
                });

            migrationBuilder.CreateTable(
                name: "Permisos",
                columns: table => new
                {
                    IdPermisos = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RolPermisos = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Permisos__CE7ED38DD4DEF2CC", x => x.IdPermisos);
                });

            migrationBuilder.CreateTable(
                name: "Proveedores",
                columns: table => new
                {
                    IdProveedor = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoPersona = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    TipoDocumento = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    NumDocumento = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    NombreCompleto = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Celular = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Ciudad = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Direccion = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Proveedo__E8B631AF01BAF91F", x => x.IdProveedor);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    IdRol = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rol = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Descripcion = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Roles__2A49584C1259A8E6", x => x.IdRol);
                });

            migrationBuilder.CreateTable(
                name: "Pedidos",
                columns: table => new
                {
                    IdPedido = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCliente = table.Column<int>(type: "int", nullable: true),
                    MetodoPago = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    FechaPedido = table.Column<DateOnly>(type: "date", nullable: true),
                    FechaEntrega = table.Column<DateOnly>(type: "date", nullable: true),
                    Descripcion = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    ValorInicial = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    ValorRestante = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    ComprobantePago = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    TotalPedido = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    IdEstado = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Pedidos__9D335DC3807CE14F", x => x.IdPedido);
                    table.ForeignKey(
                        name: "FK__Pedidos__IdClien__4E88ABD4",
                        column: x => x.IdCliente,
                        principalTable: "Clientes",
                        principalColumn: "IdCliente");
                    table.ForeignKey(
                        name: "FK__Pedidos__IdEstad__4F7CD00D",
                        column: x => x.IdEstado,
                        principalTable: "Estados_Pedido",
                        principalColumn: "IdEstadoPedidos");
                });

            migrationBuilder.CreateTable(
                name: "Produccion",
                columns: table => new
                {
                    IdProduccion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreProduccion = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    TipoProduccion = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    FechaRegistro = table.Column<DateOnly>(type: "date", nullable: true),
                    FechaFinal = table.Column<DateOnly>(type: "date", nullable: true),
                    EstadosPedido = table.Column<int>(type: "int", nullable: true),
                    IdEstado = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Producci__3137BD8383774E96", x => x.IdProduccion);
                    table.ForeignKey(
                        name: "FK__Produccio__Estad__59063A47",
                        column: x => x.EstadosPedido,
                        principalTable: "Estados_Pedido",
                        principalColumn: "IdEstadoPedidos");
                    table.ForeignKey(
                        name: "FK__Produccio__IdEst__5812160E",
                        column: x => x.IdEstado,
                        principalTable: "Estados_Produccion",
                        principalColumn: "IdEstadoProduccion");
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    IdProducto = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoriaProducto = table.Column<int>(type: "int", nullable: true),
                    Nombre = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Imagen = table.Column<int>(type: "int", nullable: true),
                    Cantidad = table.Column<int>(type: "int", nullable: true),
                    Marca = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Precio = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Producto__09889210FFDA10C6", x => x.IdProducto);
                    table.ForeignKey(
                        name: "FK__Productos__Categ__48CFD27E",
                        column: x => x.CategoriaProducto,
                        principalTable: "Categoria_Productos",
                        principalColumn: "IdCategoriaProducto");
                    table.ForeignKey(
                        name: "FK__Productos__Image__49C3F6B7",
                        column: x => x.Imagen,
                        principalTable: "Imagenes_Productos",
                        principalColumn: "IdImagen");
                });

            migrationBuilder.CreateTable(
                name: "Insumos",
                columns: table => new
                {
                    IdInsumo = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCatInsumo = table.Column<int>(type: "int", nullable: true),
                    IdMarca = table.Column<int>(type: "int", nullable: true),
                    Nombre = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Descripcion = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    Cantidad = table.Column<int>(type: "int", nullable: true),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Insumos__F378A2AFCBC81034", x => x.IdInsumo);
                    table.ForeignKey(
                        name: "FK__Insumos__IdCatIn__6477ECF3",
                        column: x => x.IdCatInsumo,
                        principalTable: "Categoria_Insumo",
                        principalColumn: "IdCatInsumo");
                    table.ForeignKey(
                        name: "FK__Insumos__IdMarca__656C112C",
                        column: x => x.IdMarca,
                        principalTable: "Marca",
                        principalColumn: "IdMarca");
                });

            migrationBuilder.CreateTable(
                name: "Compras",
                columns: table => new
                {
                    IdCompra = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdProveedor = table.Column<int>(type: "int", nullable: true),
                    MetodoPago = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    FechaCompra = table.Column<DateOnly>(type: "date", nullable: true),
                    Total = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    IdMarca = table.Column<int>(type: "int", nullable: true),
                    IdEstado = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Compras__0A5CDB5C8FEC0F2F", x => x.IdCompra);
                    table.ForeignKey(
                        name: "FK__Compras__IdEstad__6E01572D",
                        column: x => x.IdEstado,
                        principalTable: "Estado_Compra",
                        principalColumn: "IdEstado");
                    table.ForeignKey(
                        name: "FK__Compras__IdMarca__6D0D32F4",
                        column: x => x.IdMarca,
                        principalTable: "Marca",
                        principalColumn: "IdMarca");
                    table.ForeignKey(
                        name: "FK__Compras__IdProve__6C190EBB",
                        column: x => x.IdProveedor,
                        principalTable: "Proveedores",
                        principalColumn: "IdProveedor");
                });

            migrationBuilder.CreateTable(
                name: "Rol_Permisos",
                columns: table => new
                {
                    IdRol = table.Column<int>(type: "int", nullable: false),
                    IdPermisos = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rol_Permisos", x => new { x.IdRol, x.IdPermisos });
                    table.ForeignKey(
                        name: "FK_Rol_Permisos_Permisos_IdPermisos",
                        column: x => x.IdPermisos,
                        principalTable: "Permisos",
                        principalColumn: "IdPermisos",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rol_Permisos_Roles_IdRol",
                        column: x => x.IdRol,
                        principalTable: "Roles",
                        principalColumn: "IdRol",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    IdUsuarios = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCompleto = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    TipoDocumento = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    NumDocumento = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Celular = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Departamento = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Ciudad = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Direccion = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Correo = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    Contrasena = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true),
                    IdRol = table.Column<int>(type: "int", nullable: true),
                    Estado = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Usuarios__EAEBAC8F208537F1", x => x.IdUsuarios);
                    table.ForeignKey(
                        name: "FK__Usuarios__IdRol__403A8C7D",
                        column: x => x.IdRol,
                        principalTable: "Roles",
                        principalColumn: "IdRol");
                });

            migrationBuilder.CreateTable(
                name: "Detalle_Pedido",
                columns: table => new
                {
                    IdDetallePedido = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPedido = table.Column<int>(type: "int", nullable: true),
                    IdProducto = table.Column<int>(type: "int", nullable: true),
                    Cantidad = table.Column<int>(type: "int", nullable: true),
                    Subtotal = table.Column<decimal>(type: "decimal(18,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Detalle___48AFFD956BFCC7AE", x => x.IdDetallePedido);
                    table.ForeignKey(
                        name: "FK_DetallePedido_Pedido",
                        column: x => x.IdPedido,
                        principalTable: "Pedidos",
                        principalColumn: "IdPedido",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetallePedido_Producto",
                        column: x => x.IdProducto,
                        principalTable: "Productos",
                        principalColumn: "IdProducto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Detalle_Produccion",
                columns: table => new
                {
                    IdDetalleProduccion = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdProduccion = table.Column<int>(type: "int", nullable: true),
                    IdInsumo = table.Column<int>(type: "int", nullable: true),
                    IdProducto = table.Column<int>(type: "int", nullable: true),
                    CantidadProducir = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Detalle___2BD8C21E020B12D2", x => x.IdDetalleProduccion);
                    table.ForeignKey(
                        name: "FK_DetalleProduccion_IdInsumo",
                        column: x => x.IdInsumo,
                        principalTable: "Insumos",
                        principalColumn: "IdInsumo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetalleProduccion_IdProduccion",
                        column: x => x.IdProduccion,
                        principalTable: "Produccion",
                        principalColumn: "IdProduccion",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetalleProduccion_IdProducto",
                        column: x => x.IdProducto,
                        principalTable: "Productos",
                        principalColumn: "IdProducto",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Detalles_Compra",
                columns: table => new
                {
                    IdDetalleCompra = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdCompra = table.Column<int>(type: "int", nullable: true),
                    IdInsumo = table.Column<int>(type: "int", nullable: true),
                    Cantidad = table.Column<int>(type: "int", nullable: true),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,0)", nullable: true),
                    Subtotal = table.Column<decimal>(type: "decimal(18,0)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Detalles__E046CCBB4357A67F", x => x.IdDetalleCompra);
                    table.ForeignKey(
                        name: "FK_DetallesCompra_Compra",
                        column: x => x.IdCompra,
                        principalTable: "Compras",
                        principalColumn: "IdCompra",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DetallesCompra_Insumo",
                        column: x => x.IdInsumo,
                        principalTable: "Insumos",
                        principalColumn: "IdInsumo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Compras_IdEstado",
                table: "Compras",
                column: "IdEstado");

            migrationBuilder.CreateIndex(
                name: "IX_Compras_IdMarca",
                table: "Compras",
                column: "IdMarca");

            migrationBuilder.CreateIndex(
                name: "IX_Compras_IdProveedor",
                table: "Compras",
                column: "IdProveedor");

            migrationBuilder.CreateIndex(
                name: "IX_Detalle_Pedido_IdPedido",
                table: "Detalle_Pedido",
                column: "IdPedido");

            migrationBuilder.CreateIndex(
                name: "IX_Detalle_Pedido_IdProducto",
                table: "Detalle_Pedido",
                column: "IdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_Detalle_Produccion_IdInsumo",
                table: "Detalle_Produccion",
                column: "IdInsumo");

            migrationBuilder.CreateIndex(
                name: "IX_Detalle_Produccion_IdProduccion",
                table: "Detalle_Produccion",
                column: "IdProduccion");

            migrationBuilder.CreateIndex(
                name: "IX_Detalle_Produccion_IdProducto",
                table: "Detalle_Produccion",
                column: "IdProducto");

            migrationBuilder.CreateIndex(
                name: "IX_Detalles_Compra_IdCompra",
                table: "Detalles_Compra",
                column: "IdCompra");

            migrationBuilder.CreateIndex(
                name: "IX_Detalles_Compra_IdInsumo",
                table: "Detalles_Compra",
                column: "IdInsumo");

            migrationBuilder.CreateIndex(
                name: "IX_Insumos_IdCatInsumo",
                table: "Insumos",
                column: "IdCatInsumo");

            migrationBuilder.CreateIndex(
                name: "IX_Insumos_IdMarca",
                table: "Insumos",
                column: "IdMarca");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_IdCliente",
                table: "Pedidos",
                column: "IdCliente");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_IdEstado",
                table: "Pedidos",
                column: "IdEstado");

            migrationBuilder.CreateIndex(
                name: "IX_Produccion_EstadosPedido",
                table: "Produccion",
                column: "EstadosPedido");

            migrationBuilder.CreateIndex(
                name: "IX_Produccion_IdEstado",
                table: "Produccion",
                column: "IdEstado");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_CategoriaProducto",
                table: "Productos",
                column: "CategoriaProducto");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_Imagen",
                table: "Productos",
                column: "Imagen");

            migrationBuilder.CreateIndex(
                name: "IX_Rol_Permisos_IdPermisos",
                table: "Rol_Permisos",
                column: "IdPermisos");

            migrationBuilder.CreateIndex(
                name: "IX_Usuarios_IdRol",
                table: "Usuarios",
                column: "IdRol");

            migrationBuilder.CreateIndex(
                name: "UQ_Correo_NumDocumento",
                table: "Usuarios",
                columns: new[] { "Correo", "NumDocumento" },
                unique: true,
                filter: "[Correo] IS NOT NULL AND [NumDocumento] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Detalle_Pedido");

            migrationBuilder.DropTable(
                name: "Detalle_Produccion");

            migrationBuilder.DropTable(
                name: "Detalles_Compra");

            migrationBuilder.DropTable(
                name: "Rol_Permisos");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Pedidos");

            migrationBuilder.DropTable(
                name: "Produccion");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "Compras");

            migrationBuilder.DropTable(
                name: "Insumos");

            migrationBuilder.DropTable(
                name: "Permisos");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Estados_Pedido");

            migrationBuilder.DropTable(
                name: "Estados_Produccion");

            migrationBuilder.DropTable(
                name: "Categoria_Productos");

            migrationBuilder.DropTable(
                name: "Imagenes_Productos");

            migrationBuilder.DropTable(
                name: "Estado_Compra");

            migrationBuilder.DropTable(
                name: "Proveedores");

            migrationBuilder.DropTable(
                name: "Categoria_Insumo");

            migrationBuilder.DropTable(
                name: "Marca");
        }
    }
}
