using Api_CreartNino.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api_CreartNino.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProduccionController : ControllerBase
    {

        private readonly CreartNinoContext dbContext;
        public ProduccionController(CreartNinoContext context)
        {
            dbContext = context;
        }

        // GET: api/Productos
        [HttpGet]
        [Route("Lista")]

        public async Task<IActionResult> Get()
        {
            var listaproduccion = await dbContext.Produccions.ToListAsync();
            return StatusCode(StatusCodes.Status200OK, listaproduccion);
        }

        // GET: Productos/Obtener/5
        [HttpGet("Obtener/{id:int}")]
        public async Task<IActionResult> Obtener(int id)
        {
            var produccion = await dbContext.Produccions.FindAsync(id);
            if (produccion == null)
            {
                return NotFound(new { mensaje = " Produccion no encontrado." });
            }
            return Ok(produccion);
        }

        // POST: Producto/Crear
        [HttpPost("Crear")]
        public async Task<IActionResult> Crear([FromBody] Produccion objeto)
        {
            if (objeto == null)
            {
                return BadRequest(new { mensaje = "Datos inválidos." });
            }

            await dbContext.Produccions.AddAsync(objeto);
            await dbContext.SaveChangesAsync();

            return Ok(new { mensaje = " Producto creado correctamente.", objeto.IdProduccion });
        }

        // PUT: Producto/Actualizar/5
        [HttpPut]
        [Route("Actualizar/{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Produccion objeto)
        {
            if (id != objeto.IdProduccion)
            {
                return BadRequest("El ID en la URL no coincide con el ID del objeto.");
            }

            dbContext.Produccions.Update(objeto);
            await dbContext.SaveChangesAsync();
            return Ok(new { mensaje = " Produccion Actualizada correctamente" });
        }

        [HttpDelete("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var produccion = await dbContext.Produccions.FindAsync(id);
            if (produccion == null)
            {
                return NotFound(new { mensaje = " Produccion no encontrada." });
            }

            try
            {
                dbContext.Produccions.Remove(produccion);
                await dbContext.SaveChangesAsync();
                return Ok(new { mensaje = " Produccion eliminada correctamente." });
            }
            catch (DbUpdateException ex)
            {
                // Aquí puedes verificar más a fondo si la excepción tiene que ver con claves foráneas.
                return Conflict(new { mensaje = "No se puede eliminar la produccion porque está asociada a uno o más pedidos." });
            }
        }
    }
}
