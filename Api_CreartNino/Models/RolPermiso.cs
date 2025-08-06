using Api_CreartNino.Models;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Rol_Permisos")]
public class RolPermisos
{
    public int IdRol { get; set; }
    public int IdPermisos { get; set; }

    public virtual Role Rol { get; set; } = null!;
    public virtual Permiso Permiso { get; set; } = null!;
}
