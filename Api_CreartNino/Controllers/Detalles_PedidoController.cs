using Api_CreartNino.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api_CreartNino.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Detalles_PedidoController : ControllerBase
    {
        private readonly CreartNinoContext dbContext;

        public Detalles_PedidoController(CreartNinoContext context)
        {
            dbContext = context;
        }

        // GET: api/Detalles_...
        [HttpGet]
        [Route("Lista")]

        public async Task<IActionResult> Get()
        {
            var listadetalles = await dbContext.DetallePedidos.ToListAsync();
            return StatusCode(StatusCodes.Status200OK, listadetalles);
        }

        // GET: estado/Obtener/5
        [HttpGet("Obtener/{id:int}")]
        public async Task<IActionResult> Obtener(int id)
        {
            var Detalle = await dbContext.DetallePedidos.FindAsync(id);
            if (Detalle == null)
            {
                return NotFound(new { mensaje = "Detalle no encontrado." });
            }
            return Ok(Detalle);
        }

        // POST: estado/Crear
        [HttpPost("Crear")]
        public async Task<IActionResult> Crear([FromBody] DetallePedido objeto)
        {
            if (objeto == null)
            {
                return BadRequest(new { mensaje = "Datos inválidos." });
            }

            await dbContext.DetallePedidos.AddAsync(objeto);
            await dbContext.SaveChangesAsync();

            return Ok(new { mensaje = "Detalle creado correctamente.", objeto.IdDetallePedido });
        }

        // PUT: Proveedores/Actualizar/5
        [HttpPut]
        [Route("Actualizar/{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] DetallePedido objeto)
        {
            if (id != objeto.IdDetallePedido)
            {
                return BadRequest("El ID en la URL no coincide con el ID del objeto.");
            }

            dbContext.DetallePedidos.Update(objeto);
            await dbContext.SaveChangesAsync();
            return Ok(new { mensaje = "Detalle Actualizado correctamente" });
        }

        [HttpDelete("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var detalle = await dbContext.DetallePedidos.FindAsync(id);
            if (detalle == null)
            {
                return NotFound(new { mensaje = "Detalle no encontrado." });
            }

            try
            {
                dbContext.DetallePedidos.Remove(detalle);
                await dbContext.SaveChangesAsync();
                return Ok(new { mensaje = "Detalle eliminado correctamente." });
            }
            catch (DbUpdateException ex)
            {
                // Aquí puedes verificar más a fondo si la excepción tiene que ver con claves foráneas.
                return Conflict(new { mensaje = "No se puede eliminar el estado porque está asociada a uno o mas pedidos." });
            }

        }
    }
}
