using Api_CreartNino.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api_CreartNino.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolPermisosController : ControllerBase
{
    private readonly CreartNinoContext dbContext;

    public RolPermisosController(CreartNinoContext context)
    {
        dbContext = context;
    }

    [HttpGet("Lista")]
    public async Task<IActionResult> Get()
    {
        var lista = await dbContext.RolPermisos
            .Select(rp => new RolPermisosDto
            {
                IdRol = rp.IdRol,
                IdPermisos = rp.IdPermisos
            })
            .ToListAsync();

        return Ok(lista);
    }

    [HttpGet("Obtener")]
    public async Task<IActionResult> Obtener([FromQuery] int IdRol, [FromQuery] int IdPermisos)
    {
        var rp = await dbContext.RolPermisos
            .FirstOrDefaultAsync(r => r.IdRol == IdRol && r.IdPermisos == IdPermisos);

        if (rp == null)
            return NotFound(new { mensaje = "Relación no encontrada." });

        return Ok(new RolPermisosDto { IdRol = rp.IdRol, IdPermisos = rp.IdPermisos });
    }

    [HttpPost("Crear")]
    public async Task<IActionResult> Crear([FromBody] RolPermisosDto objeto)
    {
        if (objeto == null)
            return BadRequest(new { mensaje = "Datos inválidos." });

        var existe = await dbContext.RolPermisos.AnyAsync(rp =>
            rp.IdRol == objeto.IdRol && rp.IdPermisos == objeto.IdPermisos);

        if (existe)
            return Conflict(new { mensaje = "La relación ya existe." });

        await dbContext.RolPermisos.AddAsync(new RolPermisos
        {
            IdRol = objeto.IdRol,
            IdPermisos = objeto.IdPermisos
        });

        await dbContext.SaveChangesAsync();

        return Ok(new { mensaje = "Relación creada correctamente." });
    }

    [HttpDelete("Eliminar")]
    public async Task<IActionResult> Eliminar([FromQuery] int IdRol, [FromQuery] int IdPermisos)
    {
        var relacion = await dbContext.RolPermisos
            .FirstOrDefaultAsync(rp => rp.IdRol == IdRol && rp.IdPermisos == IdPermisos);

        if (relacion == null)
            return NotFound(new { mensaje = "Relación no encontrada." });

        dbContext.RolPermisos.Remove(relacion);
        await dbContext.SaveChangesAsync();

        return Ok(new { mensaje = "Relación eliminada correctamente." });
    }

    [HttpPut("Actualizar")]
    public async Task<IActionResult> Actualizar([FromQuery] int IdRol, [FromQuery] int IdPermisosActual, [FromQuery] int IdPermisosNuevo)
    {
        if (IdPermisosActual == IdPermisosNuevo)
            return BadRequest(new { mensaje = "El permiso nuevo es igual al actual." });

        var existente = await dbContext.RolPermisos.FindAsync(IdRol, IdPermisosActual);
        if (existente == null)
            return NotFound(new { mensaje = "La relación original no existe." });

        var yaExiste = await dbContext.RolPermisos.AnyAsync(rp => rp.IdRol == IdRol && rp.IdPermisos == IdPermisosNuevo);
        if (yaExiste)
            return Conflict(new { mensaje = "Ya existe la relación con el nuevo permiso." });

        dbContext.RolPermisos.Remove(existente);
        dbContext.RolPermisos.Add(new RolPermisos { IdRol = IdRol, IdPermisos = IdPermisosNuevo });
        await dbContext.SaveChangesAsync();

        return Ok(new { mensaje = "Relación actualizada correctamente." });
    }

    [HttpPut("ReemplazarPermisos/{idRol:int}")]
    public async Task<IActionResult> ReemplazarPermisos(int idRol, [FromBody] List<int> nuevosPermisos)
    {
        var actuales = await dbContext.RolPermisos
            .Where(rp => rp.IdRol == idRol)
            .ToListAsync();

        dbContext.RolPermisos.RemoveRange(actuales);

        var nuevasRelaciones = nuevosPermisos.Select(idPermiso => new RolPermisos
        {
            IdRol = idRol,
            IdPermisos = idPermiso
        });

        await dbContext.RolPermisos.AddRangeAsync(nuevasRelaciones);
        await dbContext.SaveChangesAsync();

        return Ok(new { mensaje = "Permisos del rol actualizados correctamente." });
    }

    [HttpGet("PermisosPorRol/{idRol:int}")]
    public async Task<IActionResult> PermisosPorRol(int idRol)
    {
        var permisos = await dbContext.RolPermisos
            .Where(rp => rp.IdRol == idRol)
            .Include(rp => rp.Permiso)
            .Select(rp => new PermisoDto
            {
                IdPermisos = rp.Permiso.IdPermisos,
                Nombre = rp.Permiso.RolPermisos
            })
            .ToListAsync();

        return Ok(permisos);
    }

    [HttpGet("RolesPorPermiso/{idPermiso:int}")]
    public async Task<IActionResult> RolesPorPermiso(int idPermiso)
    {
        var roles = await dbContext.RolPermisos
            .Where(rp => rp.IdPermisos == idPermiso)
            .Include(rp => rp.Rol)
            .Select(rp => new RoleDto
            {
                IdRol = rp.Rol.IdRol,
                Rol = rp.Rol.Rol,
                Descripcion = rp.Rol.Descripcion
            })
            .ToListAsync();

        return Ok(roles);
    }
}
