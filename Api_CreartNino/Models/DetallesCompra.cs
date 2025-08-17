using System;
using System.Collections.Generic;

namespace Api_CreartNino.Models;

public partial class DetallesCompra
{
    public int IdDetalleCompra { get; set; }

    public int? IdCompra { get; set; }

    public int? IdInsumo { get; set; }

    public int? Cantidad { get; set; }

    public int? PrecioUnitario { get; set; }

    public int? Subtotal { get; set; }

    public virtual Compra? IdCompraNavigation { get; set; }

    public virtual Insumo? IdInsumoNavigation { get; set; }
}
