using Api_CreartNino.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api_CreartNino.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Detalles_compraController : ControllerBase
    {
        private readonly CreartNinoContext dbContext;

        public Detalles_compraController(CreartNinoContext context)
        {
            dbContext = context;
        }

        // GET: api/Detalles_...
        [HttpGet]
        [Route("Lista")]

        public async Task<IActionResult> Get()
        {
            var listadetalles = await dbContext.DetallesCompras.ToListAsync();
            return StatusCode(StatusCodes.Status200OK, listadetalles);
        }

        // GET: estado/Obtener/5
        [HttpGet("Obtener/{id:int}")]
        public async Task<IActionResult> Obtener(int id)
        {
            var Detalle = await dbContext.DetallesCompras.FindAsync(id);
            if (Detalle == null)
            {
                return NotFound(new { mensaje = "Detalle no encontrado." });
            }
            return Ok(Detalle);
        }

        // POST: estado/Crear
        [HttpPost("Crear")]
        public async Task<IActionResult> Crear([FromBody] DetallesCompra objeto)
        {
            if (objeto == null)
            {
                return BadRequest(new { mensaje = "Datos inválidos." });
            }

            await dbContext.DetallesCompras.AddAsync(objeto);
            await dbContext.SaveChangesAsync();

            return Ok(new { mensaje = "Detalle creado correctamente.", objeto.IdDetalleCompra });
        }

        // PUT: Proveedores/Actualizar/5
        [HttpPut]
        [Route("Actualizar/{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] DetallesCompra objeto)
        {
            if (id != objeto.IdDetalleCompra)
            {
                return BadRequest("El ID en la URL no coincide con el ID del objeto.");
            }

            dbContext.DetallesCompras.Update(objeto);
            await dbContext.SaveChangesAsync();
            return Ok(new { mensaje = "Detalle Actualizado correctamente" });
        }

        [HttpDelete("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var detalle = await dbContext.DetallesCompras.FindAsync(id);
            if (detalle == null)
            {
                return NotFound(new { mensaje = "Detalle no encontrado." });
            }

            try
            {
                dbContext.DetallesCompras.Remove(detalle);
                await dbContext.SaveChangesAsync();
                return Ok(new { mensaje = "Detalle eliminado correctamente." });
            }
            catch (DbUpdateException ex)
            {
                // Aquí puedes verificar más a fondo si la excepción tiene que ver con claves foráneas.
                return Conflict(new { mensaje = "No se puede eliminar el estado porque está asociada a una o mas compras." });
            }

        }
    }
}
