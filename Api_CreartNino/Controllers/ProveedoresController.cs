using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using Api_CreartNino.Models;

namespace Api_CreartNino.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProveedoresController : ControllerBase
    {
        private readonly CreartNinoContext dbContext;

        public ProveedoresController(CreartNinoContext context)
        {
            dbContext = context;
        }

        // GET: api/proveedores
        [HttpGet]
        [Route("Lista")]

        public async Task<IActionResult> Get()
        {
            var listaproveedores = await dbContext.Proveedores.ToListAsync();
            return StatusCode(StatusCodes.Status200OK, listaproveedores);
        }

        // GET: CategoriaProductos/Obtener/5
        [HttpGet("Obtener/{id:int}")]
        public async Task<IActionResult> Obtener(int id)
        {
            var proveedor = await dbContext.Proveedores.FindAsync(id);
            if (proveedor == null)
            {
                return NotFound(new { mensaje = "Proveedor no encontrado." });
            }
            return Ok(proveedor);
        }

        // POST: Proveedores/Crear
        [HttpPost("Crear")]
        public async Task<IActionResult> Crear([FromBody] Proveedore objeto)
        {
            if (objeto == null)
            {
                return BadRequest(new { mensaje = "Datos inválidos." });
            }

            await dbContext.Proveedores.AddAsync(objeto);
            await dbContext.SaveChangesAsync();

            return Ok(new { mensaje = "Proveedor creado correctamente.", objeto.IdProveedor });
        }

        // PUT: Proveedores/Actualizar/5
        [HttpPut]
        [Route("Actualizar/{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Proveedore objeto)
        {
            if (id != objeto.IdProveedor)
            {
                return BadRequest("El ID en la URL no coincide con el ID del objeto.");
            }

            dbContext.Proveedores.Update(objeto);
            await dbContext.SaveChangesAsync();
            return Ok(new { mensaje = "Proveedor Actualizado correctamente" });
        }

        [HttpDelete("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var proveedor = await dbContext.Proveedores.FindAsync(id);
            if (proveedor == null)
            {
                return NotFound(new { mensaje = "proveedor no encontrado." });
            }

            try
            {
                dbContext.Proveedores.Remove(proveedor);
                await dbContext.SaveChangesAsync();
                return Ok(new { mensaje = "Proveedor eliminado correctamente." });
            }
            catch (DbUpdateException ex)
            {
                // Aquí puedes verificar más a fondo si la excepción tiene que ver con claves foráneas.
                return Conflict(new { mensaje = "No se puede eliminar el proveedor porque está asociada a una o mas compras." });
            }
        }
    }
}
