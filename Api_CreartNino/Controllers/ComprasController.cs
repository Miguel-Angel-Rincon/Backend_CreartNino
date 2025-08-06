using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using Api_CreartNino.Models;

namespace Api_CreartNino.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComprasController : ControllerBase
    {
        private readonly CreartNinoContext dbContext;

        public ComprasController(CreartNinoContext context)
        {
            dbContext = context;
        }

        // GET: api/Compras
        [HttpGet]
        [Route("Lista")]

        public async Task<IActionResult> Get()
        {
            var listacompras = await dbContext.Compras.ToListAsync();
            return StatusCode(StatusCodes.Status200OK, listacompras);
        }

        // GET: Compras/Obtener/5
        [HttpGet("Obtener/{id:int}")]
        public async Task<IActionResult> Obtener(int id)
        {
            var compra = await dbContext.Compras.FindAsync(id);
            if (compra == null)
            {
                return NotFound(new { mensaje = "Compra no encontrada." });
            }
            return Ok(compra);
        }

        // POST: Compras/Crear
        [HttpPost("Crear")]
        public async Task<IActionResult> Crear([FromBody] Compra objeto)
        {
            if (objeto == null)
            {
                return BadRequest(new { mensaje = "Datos inválidos." });
            }

            await dbContext.Compras.AddAsync(objeto);
            await dbContext.SaveChangesAsync();

            return Ok(new { mensaje = "Compra creada correctamente.", objeto.IdCompra });
        }

        // PUT: CategoriaInsumo/Actualizar/5
        [HttpPut]
        [Route("Actualizar/{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Compra objeto)
        {
            if (id != objeto.IdCompra)
            {
                return BadRequest("El ID en la URL no coincide con el ID del objeto.");
            }

            dbContext.Compras.Update(objeto);
            await dbContext.SaveChangesAsync();
            return Ok(new { mensaje = "Compra Actualizada correctamente" });
        }

        [HttpDelete("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var compra = await dbContext.Compras.FindAsync(id);
            if (compra == null)
            {
                return NotFound(new { mensaje = "Compra no encontrada." });
            }

            try
            {
                dbContext.Compras.Remove(compra);
                await dbContext.SaveChangesAsync();
                return Ok(new { mensaje = "Compra eliminada correctamente." });
            }
            catch (DbUpdateException ex)
            {
                // Aquí puedes verificar más a fondo si la excepción tiene que ver con claves foráneas.
                return Conflict(new { mensaje = "No se puede eliminar la compra  porque está asociada a un o más detalles." });
            }

        }
    }
}
