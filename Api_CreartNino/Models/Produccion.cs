using System;
using System.Collections.Generic;

namespace Api_CreartNino.Models;

public partial class Produccion
{
    public int IdProduccion { get; set; }

    public string? NombreProduccion { get; set; }

    public string? TipoProduccion { get; set; }

    public DateOnly? FechaInicio { get; set; }

    public DateOnly? FechaFinal { get; set; }

    

    public int? IdEstado { get; set; }

    public virtual ICollection<DetalleProduccion> DetalleProduccions { get; set; } = new List<DetalleProduccion>();

    

    public virtual EstadosProduccion? IdEstadoNavigation { get; set; }
}
