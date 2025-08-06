using Api_CreartNino.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api_CreartNino.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermisosController : ControllerBase
    {
        private readonly CreartNinoContext dbContext;
        public PermisosController(CreartNinoContext context)
        {
            dbContext = context;
        }

        // GET: api/estado
        [HttpGet]
        [Route("Lista")]

        public async Task<IActionResult> Get()
        {
            var permisos = await dbContext.Permisos.ToListAsync();
            return StatusCode(StatusCodes.Status200OK, permisos);
        }

        // GET: estado/Obtener/5
        [HttpGet("Obtener/{id:int}")]
        public async Task<IActionResult> Obtener(int id)
        {
            var permisos = await dbContext.Permisos.FindAsync(id);
            if (permisos == null)
            {
                return NotFound(new { mensaje = "permiso no encontrado." });
            }
            return Ok(permisos);
        }

        // POST: estado/Crear
        [HttpPost("Crear")]
        public async Task<IActionResult> Crear([FromBody] Permiso objeto)
        {
            if (objeto == null)
            {
                return BadRequest(new { mensaje = "Datos inválidos." });
            }

            await dbContext.Permisos.AddAsync(objeto);
            await dbContext.SaveChangesAsync();

            return Ok(new { mensaje = "permiso creado correctamente.", objeto.IdPermisos });
        }

        // PUT: Proveedores/Actualizar/5
        [HttpPut]
        [Route("Actualizar/{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Permiso objeto)
        {
            if (id != objeto.IdPermisos)
            {
                return BadRequest("El ID en la URL no coincide con el ID del objeto.");
            }

            dbContext.Permisos.Update(objeto);
            await dbContext.SaveChangesAsync();
            return Ok(new { mensaje = "permiso Actualizado correctamente" });
        }

        [HttpDelete("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var permisos = await dbContext.Permisos.FindAsync(id);
            if (permisos == null)
            {
                return NotFound(new { mensaje = "permiso no encontrado." });
            }

            try
            {
                dbContext.Permisos.Remove(permisos);
                await dbContext.SaveChangesAsync();
                return Ok(new { mensaje = "permiso eliminado correctamente." });
            }
            catch (DbUpdateException ex)
            {
                // Aquí puedes verificar más a fondo si la excepción tiene que ver con claves foráneas.
                return Conflict(new { mensaje = "No se puede eliminar el permiso por que esta asociado a uno o mas roles." });
            }
        }
    }

}
