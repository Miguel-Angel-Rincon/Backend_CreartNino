using System;
using System.Collections.Generic;

namespace Api_CreartNino.Models;

public partial class Permiso
{
    public int IdPermisos { get; set; }

    public string? RolPermisos { get; set; }

    public virtual ICollection<RolPermisos> RolPermiso { get; set; } = new List<RolPermisos>();


}
