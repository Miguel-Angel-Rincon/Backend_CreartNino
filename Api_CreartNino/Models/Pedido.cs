using System;
using System.Collections.Generic;

namespace Api_CreartNino.Models;

public partial class Pedido
{
    public int IdPedido { get; set; }

    public int? IdCliente { get; set; }

    public string? MetodoPago { get; set; }

    public DateTime? FechaPedido { get; set; }
    public DateTime? FechaEntrega { get; set; }


    public string? Descripcion { get; set; }

    public int? ValorInicial { get; set; }

    public int? ValorRestante { get; set; }

    public string? ComprobantePago { get; set; }

    public int? TotalPedido { get; set; }

    public int? IdEstado { get; set; }

    public virtual ICollection<DetallePedido> DetallePedidos { get; set; } = new List<DetallePedido>();

    public virtual Cliente? IdClienteNavigation { get; set; }

    public virtual EstadosPedido? IdEstadoNavigation { get; set; }
}
