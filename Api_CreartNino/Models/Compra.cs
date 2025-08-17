using System;
using System.Collections.Generic;

namespace Api_CreartNino.Models;

public partial class Compra
{
    public int IdCompra { get; set; }

    public int? IdProveedor { get; set; }

    public string? MetodoPago { get; set; }

    public DateOnly? FechaCompra { get; set; }

    public int? Total { get; set; }

   
    public int? IdEstado { get; set; }

    public virtual ICollection<DetallesCompra> DetallesCompras { get; set; } = new List<DetallesCompra>();

    public virtual EstadoCompra? IdEstadoNavigation { get; set; }


    public virtual Proveedore? IdProveedorNavigation { get; set; }
}
