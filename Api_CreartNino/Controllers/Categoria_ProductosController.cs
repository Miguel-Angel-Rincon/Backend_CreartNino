using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using Api_CreartNino.Models;
namespace Api_CreartNino.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Categoria_ProductosController : ControllerBase
    {
        private readonly CreartNinoContext dbContext;
        public Categoria_ProductosController(CreartNinoContext context)
        {
            dbContext = context;
        }
        // GET: api/Cate_Productos
        [HttpGet]
        [Route("Lista")]

        public async Task<IActionResult> Get()
        {
            var listaCateproductos = await dbContext.CategoriaProductos.ToListAsync();
            return StatusCode(StatusCodes.Status200OK, listaCateproductos);
        }

        // GET: CategoriaProductos/Obtener/5
        [HttpGet("Obtener/{id:int}")]
        public async Task<IActionResult> Obtener(int id)
        {
            var categoria = await dbContext.CategoriaProductos.FindAsync(id);
            if (categoria == null)
            {
                return NotFound(new { mensaje = "Categoría Producto no encontrada." });
            }
            return Ok(categoria);
        }

        // POST: CategoriaProducto/Crear
        [HttpPost("Crear")]
        public async Task<IActionResult> Crear([FromBody] CategoriaProducto objeto)
        {
            if (objeto == null)
            {
                return BadRequest(new { mensaje = "Datos inválidos." });
            }

            await dbContext.CategoriaProductos.AddAsync(objeto);
            await dbContext.SaveChangesAsync();

            return Ok(new { mensaje = "Categoría Producto creada correctamente.", objeto.IdCategoriaProducto });
        }

        // PUT: CategoriaProducto/Actualizar/5
        [HttpPut]
        [Route("Actualizar/{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] CategoriaProducto objeto)
        {
            if (id != objeto.IdCategoriaProducto)
            {
                return BadRequest("El ID en la URL no coincide con el ID del objeto.");
            }

            dbContext.CategoriaProductos.Update(objeto);
            await dbContext.SaveChangesAsync();
            return Ok(new { mensaje = "Categoria Producto Actualizada correctamente" });
        }

        [HttpDelete("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var categoria = await dbContext.CategoriaProductos.FindAsync(id);
            if (categoria == null)
            {
                return NotFound(new { mensaje = "Categoría Producto no encontrada." });
            }

            try
            {
                dbContext.CategoriaProductos.Remove(categoria);
                await dbContext.SaveChangesAsync();
                return Ok(new { mensaje = "Categoría Producto eliminada correctamente." });
            }
            catch (DbUpdateException ex)
            {
                // Aquí puedes verificar más a fondo si la excepción tiene que ver con claves foráneas.
                return Conflict(new { mensaje = "No se puede eliminar la categoría Producto porque está asociada a uno o más Productos." });
            }
        }
    }
}
