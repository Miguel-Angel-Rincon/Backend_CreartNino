using System;
using System.Collections.Generic;

namespace Api_CreartNino.Models;

public partial class EstadoCompra
{
    public int IdEstado { get; set; }

    public string? NombreEstado { get; set; }

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();
}
