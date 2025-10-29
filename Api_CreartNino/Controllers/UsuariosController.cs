using Api_CreartNino.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using Api_CreartNino.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace Api_CreartNino.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly CreartNinoContext dbContext;
        private readonly CorreoService _correoService;

        // Códigos en memoria (clave = correo)
        private static Dictionary<string, CodigoTemporal> codigosEnMemoria = new();

        public UsuariosController(CreartNinoContext context, CorreoService correoService)
        {
            dbContext = context;
            _correoService = correoService;
        }

        [HttpGet("Lista")]
        public async Task<IActionResult> Get()
        {
            var listaUsuarios = await dbContext.Usuarios.ToListAsync();
            return StatusCode(StatusCodes.Status200OK, listaUsuarios);
        }

        [HttpGet("perfil")]
        [Authorize] // ✅ Solo accesible si se envía JWT válido
        public async Task<IActionResult> ObtenerPerfil()
        {
            // 1. Obtener el correo desde el token
            var correo = User.FindFirst(ClaimTypes.Email)?.Value;

            // 2. Validar si se extrajo correctamente
            if (correo == null)
                return Unauthorized(new { mensaje = "Token no válido o expirado." });

            // 3. Buscar el usuario en la base de datos por su correo
            var usuario = await dbContext.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo);

            // 4. Validar si el usuario existe
            if (usuario == null)
                return NotFound(new { mensaje = "Usuario no encontrado." });

            // 5. Retornar solo los campos permitidos
            return Ok(new
            {
                idUsuarios = usuario.IdUsuarios,
                nombreCompleto = usuario.NombreCompleto,
                tipoDocumento = usuario.TipoDocumento,
                numDocumento = usuario.NumDocumento,
                celular = usuario.Celular,
                departamento = usuario.Departamento,
                ciudad = usuario.Ciudad,
                direccion = usuario.Direccion,
                correo = usuario.Correo,
                idRol = usuario.IdRol,
                estado = usuario.Estado
            });
        }


        [HttpGet("Obtener/{id:int}")]
        public async Task<IActionResult> Obtener(int id)
        {
            var usuario = await dbContext.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound(new { mensaje = "Usuario no encontrado." });
            }
            return Ok(usuario);
        }

        [HttpPost("Crear")]
        public async Task<IActionResult> Crear([FromBody] Usuario objeto)
        {
            if (objeto == null || string.IsNullOrEmpty(objeto.Correo) || string.IsNullOrEmpty(objeto.Contrasena))
            {
                return BadRequest(new { mensaje = "Datos inválidos. Correo y contraseña son requeridos." });
            }

            // Validar correo duplicado
            var existeCorreo = await dbContext.Usuarios.AnyAsync(u => u.Correo == objeto.Correo);
            if (existeCorreo)
            {
                return BadRequest(new { mensaje = "El correo ya está registrado." });
            }

            // Validar número de documento duplicado
            var existeDocumento = await dbContext.Usuarios.AnyAsync(u => u.NumDocumento == objeto.NumDocumento);
            if (existeDocumento)
            {
                return BadRequest(new { mensaje = "El número de documento ya está registrado." });
            }

            await dbContext.Usuarios.AddAsync(objeto);
            await dbContext.SaveChangesAsync();

            return Ok(new { mensaje = "Usuario creado correctamente.", objeto.IdUsuarios });
        }


        [HttpPut("Actualizar/{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Usuario objeto)
        {
            if (id != objeto.IdUsuarios)
            {
                return BadRequest(new { mensaje = "El ID en la URL no coincide con el del objeto enviado." });
            }

            var usuarioDb = await dbContext.Usuarios.FirstOrDefaultAsync(u => u.IdUsuarios == id);
            if (usuarioDb == null)
            {
                return NotFound(new { mensaje = "Usuario no encontrado." });
            }

            // Validar correo duplicado (excluyendo al usuario actual)
            var existeCorreo = await dbContext.Usuarios
                .AnyAsync(u => u.Correo == objeto.Correo && u.IdUsuarios != id);
            if (existeCorreo)
            {
                return BadRequest(new { mensaje = "El correo ya está registrado por otro usuario." });
            }

            // Validar número de documento duplicado (excluyendo al usuario actual)
            var existeDocumento = await dbContext.Usuarios
                .AnyAsync(u => u.NumDocumento == objeto.NumDocumento && u.IdUsuarios != id);
            if (existeDocumento)
            {
                return BadRequest(new { mensaje = "El número de documento ya está registrado por otro usuario." });
            }

            // ==== Actualizar datos del Usuario ====
            usuarioDb.NombreCompleto = objeto.NombreCompleto;
            usuarioDb.TipoDocumento = objeto.TipoDocumento;
            usuarioDb.NumDocumento = objeto.NumDocumento;
            usuarioDb.Celular = objeto.Celular;
            usuarioDb.Departamento = objeto.Departamento;
            usuarioDb.Ciudad = objeto.Ciudad;
            usuarioDb.Direccion = objeto.Direccion;
            usuarioDb.Correo = objeto.Correo;
            usuarioDb.IdRol = objeto.IdRol;
            usuarioDb.Estado = objeto.Estado;

            if (!string.IsNullOrEmpty(objeto.Contrasena))
            {
                usuarioDb.Contrasena = objeto.Contrasena;
            }

            dbContext.Usuarios.Update(usuarioDb);

            // ✅ Sincronizar solo el estado con el cliente (sin tocar los demás campos)
            var clienteDb = await dbContext.Clientes.FirstOrDefaultAsync(c => c.NumDocumento == objeto.NumDocumento);
            if (clienteDb != null && clienteDb.Estado != objeto.Estado)
            {
                clienteDb.Estado = objeto.Estado;
                dbContext.Clientes.Update(clienteDb);
            }

            await dbContext.SaveChangesAsync();

            return Ok(new { mensaje = "Usuario actualizado correctamente (estado sincronizado con cliente si aplica)." });
        }



        [HttpDelete("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var usuario = await dbContext.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound(new { mensaje = "Usuario no encontrado." });
            }

            using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                // Buscar si existe cliente con el mismo documento
                var cliente = await dbContext.Clientes
                    .FirstOrDefaultAsync(c => c.NumDocumento == usuario.NumDocumento);

                if (cliente != null)
                {
                    dbContext.Clientes.Remove(cliente);
                }

                dbContext.Usuarios.Remove(usuario);

                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new { mensaje = "Usuario (y cliente si existía) eliminado correctamente." });
            }
            catch (DbUpdateException)
            {
                await transaction.RollbackAsync();
                return Conflict(new { mensaje = "No se puede eliminar el usuario porque está asociado a una o más compras." });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, new { mensaje = "Ocurrió un error al eliminar el usuario.", detalle = ex.Message });
            }
        }

        // 🚀 Enviar código al correo (sin usar base de datos)
        [HttpPost("EnviarCodigoCorreo")]
        public async Task<IActionResult> EnviarCodigoCorreo([FromBody] string correo)
        {
            var codigo = new Random().Next(100000, 999999).ToString();

            codigosEnMemoria[correo] = new CodigoTemporal
            {
                Codigo = codigo,
                FechaExpiracion = DateTime.Now.AddMinutes(10)
            };

            await _correoService.EnviarCorreoAsync(correo, "Código de verificación", $"{codigo}");
            return Ok(new { mensaje = "Código enviado al correo." });
        }

        // 🚀 Verificar código ingresado por el cliente
        [HttpPost("VerificarCodigoCorreo")]
        public IActionResult VerificarCodigoCorreo([FromBody] VerificacionCorreo modelo)
        {
            if (!codigosEnMemoria.TryGetValue(modelo.Correo, out var temporal))
                return BadRequest("No se ha solicitado un código.");

            if (DateTime.Now > temporal.FechaExpiracion)
                return BadRequest("El código ha expirado.");

            if (modelo.Codigo != temporal.Codigo)
                return BadRequest("Código incorrecto.");

            codigosEnMemoria.Remove(modelo.Correo);
            return Ok(new { mensaje = "Código verificado correctamente." });
        }

        // ✅ Clases auxiliares (puedes moverlas a /Models si lo deseas)
        public class CodigoTemporal
        {
            public string Codigo { get; set; }
            public DateTime FechaExpiracion { get; set; }
        }

        
    }
}
