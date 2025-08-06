using System;
using System.Collections.Generic;

namespace Api_CreartNino.Models;

public partial class Role
{
    public int IdRol { get; set; }

    public string? Rol { get; set; }

    public string? Descripcion { get; set; }

    public bool? Estado { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();

    public virtual ICollection<RolPermisos> RolPermisos { get; set; } = new List<RolPermisos>();

}
