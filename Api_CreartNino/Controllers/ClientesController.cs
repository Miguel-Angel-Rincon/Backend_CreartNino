using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Api_CreartNino.Models;
using Microsoft.EntityFrameworkCore;

namespace Api_CreartNino.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {

        private readonly CreartNinoContext dbContext;

        public ClientesController(CreartNinoContext context)
        {
            dbContext = context;
        }

        // GET: api/proveedores
        [HttpGet]
        [Route("Lista")]

        public async Task<IActionResult> Get()
        {
            var listaclientes = await dbContext.Clientes.ToListAsync();
            return StatusCode(StatusCodes.Status200OK, listaclientes);
        }

        // GET: Clientes/Obtener/5
        [HttpGet("Obtener/{id:int}")]
        public async Task<IActionResult> Obtener(int id)
        {
            var clientes = await dbContext.Clientes.FindAsync(id);
            if (clientes == null)
            {
                return NotFound(new { mensaje = "Cliente no encontrado." });
            }
            return Ok(clientes);
        }

        // POST: Clientes/Crear
        [HttpPost("Crear")]
        public async Task<IActionResult> Crear([FromBody] Cliente objeto)
        {
            if (objeto == null)
            {
                return BadRequest(new { mensaje = "Datos inválidos." });
            }

            await dbContext.Clientes.AddAsync(objeto);
            await dbContext.SaveChangesAsync();

            return Ok(new { mensaje = "Cliente creado correctamente.", objeto.IdCliente });
        }

        // PUT: Proveedores/Actualizar/5
        [HttpPut]
        [Route("Actualizar/{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Cliente objeto)
        {
            if (id != objeto.IdCliente)
            {
                return BadRequest("El ID en la URL no coincide con el ID del objeto.");
            }

            dbContext.Clientes.Update(objeto);
            await dbContext.SaveChangesAsync();
            return Ok(new { mensaje = "Cliente Actualizado correctamente" });
        }

        [HttpDelete("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var cliente = await dbContext.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound(new { mensaje = "Cliente no encontrado." });
            }

            try
            {
                dbContext.Clientes.Remove(cliente);
                await dbContext.SaveChangesAsync();
                return Ok(new { mensaje = "Cliente eliminado correctamente." });
            }
            catch (DbUpdateException ex)
            {
                // Aquí puedes verificar más a fondo si la excepción tiene que ver con claves foráneas.
                return Conflict(new { mensaje = "No se puede eliminar el cliente porque está asociada a uno o mas pedidos." });
            }

        }
    }
}
