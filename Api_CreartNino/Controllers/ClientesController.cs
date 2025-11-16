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
            if (objeto == null || string.IsNullOrEmpty(objeto.Correo) || string.IsNullOrEmpty(objeto.NumDocumento))
            {
                return BadRequest(new { mensaje = "Datos inválidos. Correo y número de documento son requeridos." });
            }

            // Validar correo duplicado
            var existeCorreo = await dbContext.Clientes.AnyAsync(c => c.Correo == objeto.Correo);
            if (existeCorreo)
            {
                return BadRequest(new { mensaje = "El correo ya está registrado." });
            }

            // Validar número de documento duplicado
            var existeDocumento = await dbContext.Clientes.AnyAsync(c => c.NumDocumento == objeto.NumDocumento);
            if (existeDocumento)
            {
                return BadRequest(new { mensaje = "El número de documento ya está registrado." });
            }

            await dbContext.Clientes.AddAsync(objeto);
            await dbContext.SaveChangesAsync();

            return Ok(new { mensaje = "Cliente creado correctamente.", objeto.IdCliente });
        }


        // PUT: Proveedores/Actualizar/5
        [HttpPut("Actualizar/{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Cliente objeto)
        {
            if (id != objeto.IdCliente)
            {
                return BadRequest(new { mensaje = "El ID en la URL no coincide con el ID del objeto." });
            }

            var clienteDb = await dbContext.Clientes.FirstOrDefaultAsync(c => c.IdCliente == id);
            if (clienteDb == null)
            {
                return NotFound(new { mensaje = "Cliente no encontrado." });
            }

            // Guardar documento ORIGINAL antes de actualizar
            string documentoOriginal = clienteDb.NumDocumento;

            // Validar correo duplicado (excluyendo al cliente actual)
            var existeCorreo = await dbContext.Clientes
                .AnyAsync(c => c.Correo == objeto.Correo && c.IdCliente != id);
            if (existeCorreo)
            {
                return BadRequest(new { mensaje = "El correo ya está registrado por otro cliente." });
            }

            // Validar documento duplicado (excluyendo al cliente actual)
            var existeDocumento = await dbContext.Clientes
                .AnyAsync(c => c.NumDocumento == objeto.NumDocumento && c.IdCliente != id);
            if (existeDocumento)
            {
                return BadRequest(new { mensaje = "El número de documento ya está registrado por otro cliente." });
            }

            // === Actualizar Cliente ===
            clienteDb.NombreCompleto = objeto.NombreCompleto;
            clienteDb.TipoDocumento = objeto.TipoDocumento;
            clienteDb.NumDocumento = objeto.NumDocumento;
            clienteDb.Celular = objeto.Celular;
            clienteDb.Departamento = objeto.Departamento;
            clienteDb.Ciudad = objeto.Ciudad;
            clienteDb.Direccion = objeto.Direccion;
            clienteDb.Correo = objeto.Correo;
            clienteDb.Estado = objeto.Estado;

            dbContext.Clientes.Update(clienteDb);

            // === Sincronizar con Usuario ===
            var usuarioDb = await dbContext.Usuarios
                .FirstOrDefaultAsync(u => u.NumDocumento == documentoOriginal); // <-- IMPORTANTE

            if (usuarioDb != null)
            {
                usuarioDb.NombreCompleto = objeto.NombreCompleto;
                usuarioDb.NumDocumento = objeto.NumDocumento;
                usuarioDb.Celular = objeto.Celular;
                usuarioDb.Departamento = objeto.Departamento;
                usuarioDb.Ciudad = objeto.Ciudad;
                usuarioDb.Direccion = objeto.Direccion;
                usuarioDb.Correo = objeto.Correo;
                usuarioDb.Estado = objeto.Estado;

                dbContext.Usuarios.Update(usuarioDb);
            }

            await dbContext.SaveChangesAsync();

            return Ok(new { mensaje = "Cliente y usuario actualizados correctamente." });
        }





        [HttpDelete("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var cliente = await dbContext.Clientes.FindAsync(id);
            if (cliente == null)
            {
                return NotFound(new { mensaje = "Cliente no encontrado." });
            }

            using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                // Buscar si existe usuario con el mismo documento
                var usuario = await dbContext.Usuarios
                    .FirstOrDefaultAsync(u => u.NumDocumento == cliente.NumDocumento);

                if (usuario != null)
                {
                    dbContext.Usuarios.Remove(usuario);
                }

                dbContext.Clientes.Remove(cliente);

                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new { mensaje = "Cliente (y usuario si existía) eliminado correctamente." });
            }
            catch (DbUpdateException)
            {
                await transaction.RollbackAsync();
                return Conflict(new { mensaje = "No se puede eliminar el cliente porque está asociado a uno o más pedidos." });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { mensaje = "Ocurrió un error al eliminar el cliente.", detalle = ex.Message });
            }
        }


    }
}
