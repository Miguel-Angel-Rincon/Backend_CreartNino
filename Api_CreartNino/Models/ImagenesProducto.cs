using System;
using System.Collections.Generic;

namespace Api_CreartNino.Models;

public partial class ImagenesProducto
{
    public int IdImagen { get; set; }

    public string? Url { get; set; }

    public string? Descripcion { get; set; }

    public virtual ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
