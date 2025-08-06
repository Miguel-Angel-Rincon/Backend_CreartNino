using Api_CreartNino.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api_CreartNino.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Detalles_ProduccionController : ControllerBase
    {
        private readonly CreartNinoContext dbContext;

        public Detalles_ProduccionController(CreartNinoContext context)
        {
            dbContext = context;
        }

        // GET: api/Detalles_...
        [HttpGet]
        [Route("Lista")]

        public async Task<IActionResult> Get()
        {
            var listadetalles = await dbContext.DetalleProduccions.ToListAsync();
            return StatusCode(StatusCodes.Status200OK, listadetalles);
        }

        // GET: estado/Obtener/5
        [HttpGet("Obtener/{id:int}")]
        public async Task<IActionResult> Obtener(int id)
        {
            var Detalle = await dbContext.DetalleProduccions.FindAsync(id);
            if (Detalle == null)
            {
                return NotFound(new { mensaje = "Detalle no encontrado." });
            }
            return Ok(Detalle);
        }

        // POST: estado/Crear
        [HttpPost("Crear")]
        public async Task<IActionResult> Crear([FromBody] DetalleProduccion objeto)
        {
            if (objeto == null)
            {
                return BadRequest(new { mensaje = "Datos inválidos." });
            }

            await dbContext.DetalleProduccions.AddAsync(objeto);
            await dbContext.SaveChangesAsync();

            return Ok(new { mensaje = "Detalle creado correctamente.", objeto.IdDetalleProduccion });
        }

        // PUT: Proveedores/Actualizar/5
        [HttpPut]
        [Route("Actualizar/{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] DetalleProduccion objeto)
        {
            if (id != objeto.IdDetalleProduccion)
            {
                return BadRequest("El ID en la URL no coincide con el ID del objeto.");
            }

            dbContext.DetalleProduccions.Update(objeto);
            await dbContext.SaveChangesAsync();
            return Ok(new { mensaje = "Detalle Actualizado correctamente" });
        }

        [HttpDelete("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var detalle = await dbContext.DetalleProduccions.FindAsync(id);
            if (detalle == null)
            {
                return NotFound(new { mensaje = "Detalle no encontrado." });
            }

            try
            {
                dbContext.DetalleProduccions.Remove(detalle);
                await dbContext.SaveChangesAsync();
                return Ok(new { mensaje = "Detalle eliminado correctamente." });
            }
            catch (DbUpdateException ex)
            {
                // Aquí puedes verificar más a fondo si la excepción tiene que ver con claves foráneas.
                return Conflict(new { mensaje = "No se puede eliminar el estado porque está asociada a una o mas producciones." });
            }

        }
    }
}
