using System;
using System.Collections.Generic;

namespace Api_CreartNino.Models;

public partial class VwRolesPermiso
{
    public int IdRol { get; set; }

    public string? Rol { get; set; }

    public string? Descripcion { get; set; }

    public int IdPermisos { get; set; }

    public string? RolPermisos { get; set; }
}
