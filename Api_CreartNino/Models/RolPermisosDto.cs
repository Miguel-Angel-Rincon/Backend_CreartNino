// DTO para relacion Rol - Permisos
public class RolPermisosDto
{
    public int IdRol { get; set; }
    public int IdPermisos { get; set; }
}

// DTO para Permiso
public class PermisoDto
{
    public int IdPermisos { get; set; }
    public string Nombre { get; set; } = string.Empty;
}

// DTO para Rol
public class RoleDto
{
    public int IdRol { get; set; }
    public string Rol { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
}
