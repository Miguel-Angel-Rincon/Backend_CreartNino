using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Api_CreartNino.Models;

public partial class CreartNinoContext : DbContext
{
    public CreartNinoContext()
    {
    }

    public CreartNinoContext(DbContextOptions<CreartNinoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<CategoriaInsumo> CategoriaInsumos { get; set; }

    public virtual DbSet<CategoriaProducto> CategoriaProductos { get; set; }

    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Compra> Compras { get; set; }

    public virtual DbSet<DetallePedido> DetallePedidos { get; set; }

    public virtual DbSet<DetalleProduccion> DetalleProduccions { get; set; }

    public virtual DbSet<DetallesCompra> DetallesCompras { get; set; }

    public virtual DbSet<EstadoCompra> EstadoCompras { get; set; }

    public virtual DbSet<EstadosPedido> EstadosPedidos { get; set; }

    public virtual DbSet<EstadosProduccion> EstadosProduccions { get; set; }

    public virtual DbSet<ImagenesProducto> ImagenesProductos { get; set; }

    public virtual DbSet<Insumo> Insumos { get; set; }


    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<Permiso> Permisos { get; set; }

    public virtual DbSet<Produccion> Produccions { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Proveedore> Proveedores { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public virtual DbSet<VwRolesPermiso> VwRolesPermisos { get; set; }

    public virtual DbSet<RolPermisos> RolPermisos { get; set; }

//    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//        => optionsBuilder.UseSqlServer("Server=DESKTOP-I7ON295;Initial Catalog=CreartNino;integrated security=True; TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.Entity<RolPermisos>()
            .ToTable("Rol_Permisos") 
                .HasKey(rp => new { rp.IdRol, rp.IdPermisos });

        modelBuilder.Entity<RolPermisos>()
            .HasOne(rp => rp.Rol)
            .WithMany(r => r.RolPermisos)
            .HasForeignKey(rp => rp.IdRol)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RolPermisos>()
            .HasOne(rp => rp.Permiso)
            .WithMany(p => p.RolPermiso)
            .HasForeignKey(rp => rp.IdPermisos)
            .OnDelete(DeleteBehavior.Cascade);
    
                

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__Roles__2A49584C1259A8E6");
            entity.Property(e => e.Descripcion).HasMaxLength(100).IsUnicode(false);
            entity.Property(e => e.Rol).HasMaxLength(50).IsUnicode(false);
        });

        modelBuilder.Entity<Permiso>(entity =>
        {
            entity.HasKey(e => e.IdPermisos).HasName("PK__Permisos__CE7ED38DD4DEF2CC");
            entity.Property(e => e.RolPermisos).HasMaxLength(50).IsUnicode(false);
        });

        modelBuilder.Entity<CategoriaInsumo>(entity =>
        {
            entity.HasKey(e => e.IdCatInsumo).HasName("PK__Categori__6815ADDF91908ECC");

            entity.ToTable("Categoria_Insumo");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NombreCategoria)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CategoriaProducto>(entity =>
        {
            entity.HasKey(e => e.IdCategoriaProducto).HasName("PK__Categori__8A4F21C36F377EF0");

            entity.ToTable("Categoria_Productos");

            entity.Property(e => e.CategoriaProducto1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CategoriaProducto");
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.IdCliente).HasName("PK__Clientes__D5946642051B7E6E");

            entity.Property(e => e.Celular)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Ciudad)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Correo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Departamento)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Direccion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NombreCompleto)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NumDocumento)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TipoDocumento)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Compra>(entity =>
        {
            entity.HasKey(e => e.IdCompra).HasName("PK__Compras__0A5CDB5C8FEC0F2F");

            entity.Property(e => e.MetodoPago)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Total).HasColumnType("int");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.IdEstado)
                .HasConstraintName("FK__Compras__IdEstad__6E01572D");

            

            entity.HasOne(d => d.IdProveedorNavigation).WithMany(p => p.Compras)
                .HasForeignKey(d => d.IdProveedor)
                .HasConstraintName("FK__Compras__IdProve__6C190EBB");
        });

        modelBuilder.Entity<DetallePedido>(entity =>
        {
            entity.HasKey(e => e.IdDetallePedido).HasName("PK__Detalle___48AFFD956BFCC7AE");

            entity.ToTable("Detalle_Pedido");

            entity.Property(e => e.Subtotal).HasColumnType("int");

            entity.HasOne(d => d.IdPedidoNavigation).WithMany(p => p.DetallePedidos)
                .HasForeignKey(d => d.IdPedido)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_DetallePedido_Pedido");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.DetallePedidos)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_DetallePedido_Producto");
        });

        modelBuilder.Entity<DetalleProduccion>(entity =>
        {
            entity.HasKey(e => e.IdDetalleProduccion).HasName("PK__Detalle___2BD8C21E020B12D2");

            entity.ToTable("Detalle_Produccion");

            entity.HasOne(d => d.IdInsumoNavigation).WithMany(p => p.DetalleProduccions)
                .HasForeignKey(d => d.IdInsumo)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_DetalleProduccion_IdInsumo");

            entity.HasOne(d => d.IdProduccionNavigation).WithMany(p => p.DetalleProduccions)
                .HasForeignKey(d => d.IdProduccion)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_DetalleProduccion_IdProduccion");

            entity.HasOne(d => d.IdProductoNavigation).WithMany(p => p.DetalleProduccions)
                .HasForeignKey(d => d.IdProducto)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_DetalleProduccion_IdProducto");

            modelBuilder.Entity<DetalleProduccion>()
    .HasOne(d => d.IdPedidoNavigation)
    .WithMany()
    .HasForeignKey(d => d.IdPedido)
    .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<DetallesCompra>(entity =>
        {
            entity.HasKey(e => e.IdDetalleCompra).HasName("PK__Detalles__E046CCBB4357A67F");

            entity.ToTable("Detalles_Compra");

            entity.Property(e => e.PrecioUnitario).HasColumnType("int");
            entity.Property(e => e.Subtotal).HasColumnType("int");

            entity.HasOne(d => d.IdCompraNavigation).WithMany(p => p.DetallesCompras)
                .HasForeignKey(d => d.IdCompra)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_DetallesCompra_Compra");

            entity.HasOne(d => d.IdInsumoNavigation).WithMany(p => p.DetallesCompras)
                .HasForeignKey(d => d.IdInsumo)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_DetallesCompra_Insumo");
        });

        modelBuilder.Entity<EstadoCompra>(entity =>
        {
            entity.HasKey(e => e.IdEstado).HasName("PK__Estado_C__FBB0EDC15C474046");

            entity.ToTable("Estado_Compra");

            entity.Property(e => e.NombreEstado)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<EstadosPedido>(entity =>
        {
            entity.HasKey(e => e.IdEstadoPedidos).HasName("PK__Estados___4243CC3E3ADA014C");

            entity.ToTable("Estados_Pedido");

            entity.Property(e => e.NombreEstado)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<EstadosProduccion>(entity =>
        {
            entity.HasKey(e => e.IdEstadoProduccion).HasName("PK__Estados___6DEB350E9C367EDB");

            entity.ToTable("Estados_Produccion");

            entity.Property(e => e.NombreEstado)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ImagenesProducto>(entity =>
        {
            entity.HasKey(e => e.IdImagen).HasName("PK__Imagenes__B42D8F2A203F6994");

            entity.ToTable("Imagenes_Productos");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Url)
                .HasMaxLength(300)
                .IsUnicode(false)
                .HasColumnName("URL");
        });

        modelBuilder.Entity<Insumo>(entity =>
        {
            entity.HasKey(e => e.IdInsumo).HasName("PK__Insumos__F378A2AFCBC81034");

            entity.Property(e => e.UnidadesMedidas)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PrecioUnitario).HasColumnType("int");

            entity.HasOne(d => d.IdCatInsumoNavigation).WithMany(p => p.Insumos)
                .HasForeignKey(d => d.IdCatInsumo)
                .HasConstraintName("FK__Insumos__IdCatIn__6477ECF3");

            
        });

       

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.IdPedido).HasName("PK__Pedidos__9D335DC3807CE14F");

            entity.Property(e => e.ComprobantePago)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.MetodoPago)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TotalPedido).HasColumnType("int");
            entity.Property(e => e.ValorInicial).HasColumnType("int");
            entity.Property(e => e.ValorRestante).HasColumnType("int");

            entity.HasOne(d => d.IdClienteNavigation).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.IdCliente)
                .HasConstraintName("FK__Pedidos__IdClien__4E88ABD4");

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.IdEstado)
                .HasConstraintName("FK__Pedidos__IdEstad__4F7CD00D");
        });

        modelBuilder.Entity<Permiso>(entity =>
        {
            entity.HasKey(e => e.IdPermisos).HasName("PK__Permisos__CE7ED38DD4DEF2CC");

            entity.Property(e => e.RolPermisos)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Produccion>(entity =>
        {
            entity.HasKey(e => e.IdProduccion).HasName("PK__Producci__3137BD8383774E96");

            entity.ToTable("Produccion");

            entity.Property(e => e.NombreProduccion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TipoProduccion)
                .HasMaxLength(50)
                .IsUnicode(false);

           

            entity.HasOne(d => d.IdEstadoNavigation).WithMany(p => p.Produccions)
                .HasForeignKey(d => d.IdEstado)
                .HasConstraintName("FK__Produccio__IdEst__5812160E");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.IdProducto).HasName("PK__Producto__09889210FFDA10C6");

            entity.Property(e => e.Marca)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Precio).HasColumnType("int");

            entity.HasOne(d => d.CategoriaProductoNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.CategoriaProducto)
                .HasConstraintName("FK__Productos__Categ__48CFD27E");

            entity.HasOne(d => d.ImagenNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.Imagen)
                .HasConstraintName("FK__Productos__Image__49C3F6B7");
        });

        modelBuilder.Entity<Proveedore>(entity =>
        {
            entity.HasKey(e => e.IdProveedor).HasName("PK__Proveedo__E8B631AF01BAF91F");

            entity.Property(e => e.Celular)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Ciudad)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Direccion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NombreCompleto)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NumDocumento)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TipoDocumento)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TipoPersona)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__Roles__2A49584C1259A8E6");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Rol)
                .HasMaxLength(50)
                .IsUnicode(false);

            
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.IdUsuarios).HasName("PK__Usuarios__EAEBAC8F208537F1");

            entity.HasIndex(e => new { e.Correo, e.NumDocumento }, "UQ_Correo_NumDocumento").IsUnique();

            entity.Property(e => e.Celular)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Ciudad)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Contrasena)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Correo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Departamento)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Direccion)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NombreCompleto)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NumDocumento)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TipoDocumento)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.IdRol)
                .HasConstraintName("FK__Usuarios__IdRol__403A8C7D");
        });

        modelBuilder.Entity<VwRolesPermiso>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Roles_Permisos");

            entity.Property(e => e.Descripcion)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Rol)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RolPermisos)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
