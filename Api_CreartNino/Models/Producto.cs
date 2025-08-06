using System;
using System.Collections.Generic;

namespace Api_CreartNino.Models;

public partial class Producto
{
    public int IdProducto { get; set; }

    public int? CategoriaProducto { get; set; }

    public string? Nombre { get; set; }

    public int? Imagen { get; set; }

    public int? Cantidad { get; set; }

    public string? Marca { get; set; }

    public decimal? Precio { get; set; }

    public bool? Estado { get; set; }

    public virtual CategoriaProducto? CategoriaProductoNavigation { get; set; }

    public virtual ICollection<DetallePedido> DetallePedidos { get; set; } = new List<DetallePedido>();

    public virtual ICollection<DetalleProduccion> DetalleProduccions { get; set; } = new List<DetalleProduccion>();

    public virtual ImagenesProducto? ImagenNavigation { get; set; }
}
