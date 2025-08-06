using System;
using System.Collections.Generic;

namespace Api_CreartNino.Models;

public partial class Insumo
{
    public int IdInsumo { get; set; }

    public int? IdCatInsumo { get; set; }

    
    public string? Nombre { get; set; }

    public string? Descripcion { get; set; }

    public int? Cantidad { get; set; }

    public decimal? PrecioUnitario { get; set; }

    public bool? Estado { get; set; }

    public virtual ICollection<DetalleProduccion> DetalleProduccions { get; set; } = new List<DetalleProduccion>();

    public virtual ICollection<DetallesCompra> DetallesCompras { get; set; } = new List<DetallesCompra>();

    public virtual CategoriaInsumo? IdCatInsumoNavigation { get; set; }

    
}
