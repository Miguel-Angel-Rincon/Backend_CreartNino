using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using Api_CreartNino.Models;
namespace Api_CreartNino.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Imagenes_ProductosController : ControllerBase
    {
        private readonly CreartNinoContext dbContext;
        public Imagenes_ProductosController(CreartNinoContext context)
        {
            dbContext = context;
        }

        // GET: api/Imagenes_Productos
        [HttpGet]
        [Route("Lista")]

        public async Task<IActionResult> Get()
        {
            var listaimagenes = await dbContext.ImagenesProductos.ToListAsync();
            return StatusCode(StatusCodes.Status200OK, listaimagenes);
        }

        // GET: Imagenes_Productos/Obtener/5
        [HttpGet("Obtener/{id:int}")]
        public async Task<IActionResult> Obtener(int id)
        {
            var imagenes = await dbContext.ImagenesProductos.FindAsync(id);
            if (imagenes == null)
            {
                return NotFound(new { mensaje = "Imagen no encontrada." });
            }
            return Ok(imagenes);
        }

        // POST: Imagenes_Productos/Crear
        [HttpPost("Crear")]
        public async Task<IActionResult> Crear([FromBody] ImagenesProducto objeto)
        {
            if (objeto == null)
            {
                return BadRequest(new { mensaje = "Datos inválidos." });
            }

            await dbContext.ImagenesProductos.AddAsync(objeto);
            await dbContext.SaveChangesAsync();

            return Ok(new { mensaje = "Imagen creada correctamente.", objeto.IdImagen });
        }

        // PUT: Imagenes_Productos/Actualizar/5
        [HttpPut]
        [Route("Actualizar/{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] ImagenesProducto objeto)
        {
            if (id != objeto.IdImagen)
            {
                return BadRequest("El ID en la URL no coincide con el ID del objeto.");
            }

            dbContext.ImagenesProductos.Update(objeto);
            await dbContext.SaveChangesAsync();
            return Ok(new { mensaje = "Imagen Actualizada correctamente" });
        }

        [HttpDelete("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var proveedor = await dbContext.ImagenesProductos.FindAsync(id);
            if (proveedor == null)
            {
                return NotFound(new { mensaje = "Imagen no encontrada." });
            }

            try
            {
                dbContext.ImagenesProductos.Remove(proveedor);
                await dbContext.SaveChangesAsync();
                return Ok(new { mensaje = "Imagen eliminada correctamente." });
            }
            catch (DbUpdateException ex)
            {
                // Aquí puedes verificar más a fondo si la excepción tiene que ver con claves foráneas.
                return Conflict(new { mensaje = "No se puede eliminar la imagen porque está asociada a uno o mas productos." });
            }
        }
    }
}
