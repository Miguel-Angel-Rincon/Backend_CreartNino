using System;
using System.Collections.Generic;

namespace Api_CreartNino.Models;

public partial class DetalleProduccion
{
    public int IdDetalleProduccion { get; set; }

    public int? IdProduccion { get; set; }

    public int? IdInsumo { get; set; }

    public int? IdProducto { get; set; }

    public int? CantidadProducir { get; set; }

    public virtual Insumo? IdInsumoNavigation { get; set; }

    public virtual Produccion? IdProduccionNavigation { get; set; }

    public virtual Producto? IdProductoNavigation { get; set; }
}
