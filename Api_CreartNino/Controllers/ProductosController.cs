using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using Api_CreartNino.Models;
namespace Api_CreartNino.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly CreartNinoContext dbContext;
        public ProductosController(CreartNinoContext context)
        {
            dbContext = context;
        }

        // GET: api/Productos
        [HttpGet]
        [Route("Lista")]

        public async Task<IActionResult> Get()
        {
            var listaproductos = await dbContext.Productos.ToListAsync();
            return StatusCode(StatusCodes.Status200OK, listaproductos);
        }

        // GET: Productos/Obtener/5
        [HttpGet("Obtener/{id:int}")]
        public async Task<IActionResult> Obtener(int id)
        {
            var producto = await dbContext.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound(new { mensaje = " Producto no encontrado." });
            }
            return Ok(producto);
        }

        // POST: Producto/Crear
        [HttpPost("Crear")]
        public async Task<IActionResult> Crear([FromBody] Producto objeto)
        {
            if (objeto == null)
            {
                return BadRequest(new { mensaje = "Datos inválidos." });
            }

            await dbContext.Productos.AddAsync(objeto);
            await dbContext.SaveChangesAsync();

            return Ok(new { mensaje = " Producto creado correctamente.", objeto.IdProducto });
        }

        // PUT: Producto/Actualizar/5
        [HttpPut]
        [Route("Actualizar/{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Producto objeto)
        {
            if (id != objeto.IdProducto)
            {
                return BadRequest("El ID en la URL no coincide con el ID del objeto.");
            }

            dbContext.Productos.Update(objeto);
            await dbContext.SaveChangesAsync();
            return Ok(new { mensaje = " Producto Actualizado correctamente" });
        }

        [HttpDelete("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var producto = await dbContext.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound(new { mensaje = " Producto no encontrado." });
            }

            try
            {
                dbContext.Productos.Remove(producto);
                await dbContext.SaveChangesAsync();
                return Ok(new { mensaje = " Producto eliminado correctamente." });
            }
            catch (DbUpdateException ex)
            {
                // Aquí puedes verificar más a fondo si la excepción tiene que ver con claves foráneas.
                return Conflict(new { mensaje = "No se puede eliminar el Producto porque está asociada a uno o más pedido,producciones." });
            }
        }
    }
}
