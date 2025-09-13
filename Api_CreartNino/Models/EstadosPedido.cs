using System;
using System.Collections.Generic;

namespace Api_CreartNino.Models;

public partial class EstadosPedido
{
    public int IdEstadoPedidos { get; set; }

    public string? NombreEstado { get; set; }

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

    
}
