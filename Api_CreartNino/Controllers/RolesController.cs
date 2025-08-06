using Api_CreartNino.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api_CreartNino.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly CreartNinoContext dbContext;
        public RolesController(CreartNinoContext context)
        {
            dbContext = context;
        }

        // GET: api/estado
        [HttpGet]
        [Route("Lista")]

        public async Task<IActionResult> Get()
        {
            var Rol = await dbContext.Roles.ToListAsync();
            return StatusCode(StatusCodes.Status200OK, Rol);
        }

        // GET: estado/Obtener/5
        [HttpGet("Obtener/{id:int}")]
        public async Task<IActionResult> Obtener(int id)
        {
            var Rol = await dbContext.Roles.FindAsync(id);
            if (Rol == null)
            {
                return NotFound(new { mensaje = "Rol no encontrado." });
            }
            return Ok(Rol);
        }

        // POST: estado/Crear
        [HttpPost("Crear")]
        public async Task<IActionResult> Crear([FromBody] Role objeto)
        {
            if (objeto == null)
            {
                return BadRequest(new { mensaje = "Datos inválidos." });
            }

            await dbContext.Roles.AddAsync(objeto);
            await dbContext.SaveChangesAsync();

            return Ok(new { mensaje = "Rol creado correctamente.", objeto.IdRol });
        }

        // PUT: Proveedores/Actualizar/5
        [HttpPut]
        [Route("Actualizar/{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Role objeto)
        {
            if (id != objeto.IdRol)
            {
                return BadRequest("El ID en la URL no coincide con el ID del objeto.");
            }

            dbContext.Roles.Update(objeto);
            await dbContext.SaveChangesAsync();
            return Ok(new { mensaje = "Rol Actualizado correctamente" });
        }

        [HttpDelete("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var Rol = await dbContext.Roles.FindAsync(id);
            if (Rol == null)
            {
                return NotFound(new { mensaje = "Rol no encontrado." });
            }

            try
            {
                dbContext.Roles.Remove(Rol);
                await dbContext.SaveChangesAsync();
                return Ok(new { mensaje = "Rol eliminado correctamente." });
            }
            catch (DbUpdateException ex)
            {
                // Aquí puedes verificar más a fondo si la excepción tiene que ver con claves foráneas.
                return Conflict(new { mensaje = "No se puede eliminar el ROL porque está asociada a uno o mas Usuarios." });
            }

        }
    }
}
