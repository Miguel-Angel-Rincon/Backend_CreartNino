using System;
using System.Collections.Generic;

namespace Api_CreartNino.Models;

public partial class Cliente
{
    public int IdCliente { get; set; }

    public string? NombreCompleto { get; set; }

    public string? TipoDocumento { get; set; }

    public string? NumDocumento { get; set; }

    public string? Correo { get; set; }

    public string? Celular { get; set; }

    public string? Departamento { get; set; }

    public string? Ciudad { get; set; }

    public string? Direccion { get; set; }

    public bool? Estado { get; set; }

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
