using System;
using System.Collections.Generic;

namespace Api_CreartNino.Models;

public partial class CategoriaInsumo
{
    public int IdCatInsumo { get; set; }

    public string? NombreCategoria { get; set; }

    public string? Descripcion { get; set; }

    public bool? Estado { get; set; }

    public virtual ICollection<Insumo> Insumos { get; set; } = new List<Insumo>();
}
