using System;
using System.Collections.Generic;

namespace Api_CreartNino.Models;

public partial class EstadosProduccion
{
    public int IdEstadoProduccion { get; set; }

    public string? NombreEstado { get; set; }

    public virtual ICollection<Produccion> Produccions { get; set; } = new List<Produccion>();
}
