using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using Api_CreartNino.Models;
namespace Api_CreartNino.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsumosController : ControllerBase
    {
        private readonly CreartNinoContext dbContext;

        public InsumosController(CreartNinoContext context)
        {
            dbContext = context;
        }

        // GET: api/Insumos
        [HttpGet]
        [Route("Lista")]

        public async Task<IActionResult> Get()
        {
            var listaCateInsumos = await dbContext.Insumos.ToListAsync();
            return StatusCode(StatusCodes.Status200OK, listaCateInsumos);
        }

        // GET: CategoriaInsumo/Obtener/5
        [HttpGet("Obtener/{id:int}")]
        public async Task<IActionResult> Obtener(int id)
        {
            var categoria = await dbContext.Insumos.FindAsync(id);
            if (categoria == null)
            {
                return NotFound(new { mensaje = " Insumo no encontrado." });
            }
            return Ok(categoria);
        }

        // POST: CategoriaInsumo/Crear
        [HttpPost("Crear")]
        public async Task<IActionResult> Crear([FromBody] Insumo objeto)
        {
            if (objeto == null)
            {
                return BadRequest(new { mensaje = "Datos inválidos." });
            }

            await dbContext.Insumos.AddAsync(objeto);
            await dbContext.SaveChangesAsync();

            return Ok(new { mensaje = " Insumo creado correctamente.", objeto.IdInsumo });
        }

        // PUT: CategoriaInsumo/Actualizar/5
        [HttpPut]
        [Route("Actualizar/{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Insumo objeto)
        {
            if (id != objeto.IdInsumo)
            {
                return BadRequest("El ID en la URL no coincide con el ID del objeto.");
            }

            dbContext.Insumos.Update(objeto);
            await dbContext.SaveChangesAsync();
            return Ok(new { mensaje = " Insumo Actualizado correctamente" });
        }

        [HttpDelete("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var insumo = await dbContext.Insumos.FindAsync(id);
            if (insumo == null)
            {
                return NotFound(new { mensaje = "Insumo no encontrado." });
            }

            try
            {
                dbContext.Insumos.Remove(insumo);
                await dbContext.SaveChangesAsync();
                return Ok(new { mensaje = " Insumo eliminado correctamente." });
            }
            catch (DbUpdateException ex)
            {
                // Aquí puedes verificar más a fondo si la excepción tiene que ver con claves foráneas.
                return Conflict(new { mensaje = "No se puede eliminar el Insumo porque está asociada a uno o más producciones." });
            }
        }
    }
}
