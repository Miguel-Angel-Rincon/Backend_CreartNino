using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using Api_CreartNino.Models;

namespace Api_CreartNino.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Estados_CompraController : ControllerBase
    {
        private readonly CreartNinoContext dbContext;
        public Estados_CompraController(CreartNinoContext context)
        {
            dbContext = context;
        }


        // GET: api/estado
        [HttpGet]
        [Route("Lista")]

        public async Task<IActionResult> Get()
        {
            var listaEstados = await dbContext.EstadoCompras.ToListAsync();
            return StatusCode(StatusCodes.Status200OK, listaEstados);
        }

        // GET: estado/Obtener/5
        [HttpGet("Obtener/{id:int}")]
        public async Task<IActionResult> Obtener(int id)
        {
            var estado = await dbContext.EstadoCompras.FindAsync(id);
            if (estado == null)
            {
                return NotFound(new { mensaje = "Estado no encontrado." });
            }
            return Ok(estado);
        }

        // POST: estado/Crear
        [HttpPost("Crear")]
        public async Task<IActionResult> Crear([FromBody] EstadoCompra objeto)
        {
            if (objeto == null)
            {
                return BadRequest(new { mensaje = "Datos inválidos." });
            }

            await dbContext.EstadoCompras.AddAsync(objeto);
            await dbContext.SaveChangesAsync();

            return Ok(new { mensaje = "Estado creado correctamente.", objeto.IdEstado });
        }

        // PUT: Proveedores/Actualizar/5
        [HttpPut]
        [Route("Actualizar/{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] EstadoCompra objeto)
        {
            if (id != objeto.IdEstado)
            {
                return BadRequest("El ID en la URL no coincide con el ID del objeto.");
            }

            dbContext.EstadoCompras.Update(objeto);
            await dbContext.SaveChangesAsync();
            return Ok(new { mensaje = "Estado Actualizado correctamente" });
        }

        [HttpDelete("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var estado = await dbContext.EstadoCompras.FindAsync(id);
            if (estado == null)
            {
                return NotFound(new { mensaje = "Estado no encontrado." });
            }

            try
            {
                dbContext.EstadoCompras.Remove(estado);
                await dbContext.SaveChangesAsync();
                return Ok(new { mensaje = "Estado eliminado correctamente." });
            }
            catch (DbUpdateException ex)
            {
                // Aquí puedes verificar más a fondo si la excepción tiene que ver con claves foráneas.
                return Conflict(new { mensaje = "No se puede eliminar el estado porque está asociada a una o mas compras." });
            }

        }
    }
}
