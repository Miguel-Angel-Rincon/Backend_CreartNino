using Api_CreartNino.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api_CreartNino.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Categoria_InsumosController : ControllerBase
    {
        private readonly CreartNinoContext dbContext;
        public Categoria_InsumosController(CreartNinoContext context)
        {
            dbContext = context;
        }

        // GET: api/Cate_Insumos
        [HttpGet]
        [Route("Lista")]

        public async Task<IActionResult> Get()
        {
            var listaCateInsumos = await dbContext.CategoriaInsumos.ToListAsync();
            return StatusCode(StatusCodes.Status200OK, listaCateInsumos);
        }

        // GET: CategoriaInsumo/Obtener/5
        [HttpGet("Obtener/{id:int}")]
        public async Task<IActionResult> Obtener(int id)
        {
            var categoria = await dbContext.CategoriaInsumos.FindAsync(id);
            if (categoria == null)
            {
                return NotFound(new { mensaje = "Categoría Insumo no encontrada." });
            }
            return Ok(categoria);
        }

        // POST: CategoriaInsumo/Crear
        [HttpPost("Crear")]
        public async Task<IActionResult> Crear([FromBody] CategoriaInsumo objeto)
        {
            if (objeto == null)
            {
                return BadRequest(new { mensaje = "Datos inválidos." });
            }

            await dbContext.CategoriaInsumos.AddAsync(objeto);
            await dbContext.SaveChangesAsync();

            return Ok(new { mensaje = "Categoría Insumo creada correctamente.", objeto.IdCatInsumo });
        }

        // PUT: CategoriaInsumo/Actualizar/5
        [HttpPut]
        [Route("Actualizar/{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] CategoriaInsumo objeto)
        {
            if (id != objeto.IdCatInsumo)
            {
                return BadRequest("El ID en la URL no coincide con el ID del objeto.");
            }

            dbContext.CategoriaInsumos.Update(objeto);
            await dbContext.SaveChangesAsync();
            return Ok(new { mensaje = "Categoria Insumo Actualizada correctamente" });
        }

        [HttpDelete("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var categoria = await dbContext.CategoriaInsumos.FindAsync(id);
            if (categoria == null)
            {
                return NotFound(new { mensaje = "Categoría Insumo no encontrada." });
            }

            try
            {
                dbContext.CategoriaInsumos.Remove(categoria);
                await dbContext.SaveChangesAsync();
                return Ok(new { mensaje = "Categoría Insumo eliminada correctamente." });
            }
            catch (DbUpdateException ex)
            {
                // Aquí puedes verificar más a fondo si la excepción tiene que ver con claves foráneas.
                return Conflict(new { mensaje = "No se puede eliminar la categoría Insumo porque está asociada a uno o más insumos." });
            }
        }

    }
}
   