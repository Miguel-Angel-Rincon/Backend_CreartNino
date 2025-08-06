using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using Api_CreartNino.Models;
using Microsoft.EntityFrameworkCore;

namespace Api_CreartNino.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly CreartNinoContext _context;

        private static Dictionary<string, CodigoTemporal> codigosLogin = new();
        private static Dictionary<string, CodigoTemporal> codigosRecuperacion = new();

        public AuthController(IConfiguration config, CreartNinoContext context)
        {
            _config = config;
            _context = context;
        }

        [HttpPost("LoginPaso1")]
        public async Task<IActionResult> LoginPaso1([FromBody] LoginRequest request)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == request.Correo && u.Contrasena == request.Contrasena);
            if (usuario == null)
                return Unauthorized(new { mensaje = "Correo o contraseña incorrectos." });

            var codigo = new Random().Next(100000, 999999).ToString();
            codigosLogin[request.Correo] = new CodigoTemporal { Codigo = codigo, FechaExpiracion = DateTime.Now.AddMinutes(10) };

            var cuerpoHtml = CrearCorreoLogin(usuario.NombreCompleto, codigo);
            await EnviarCorreo(request.Correo, "Código de verificación", cuerpoHtml);
            return Ok(new { mensaje = "Código enviado al correo. Verifica para continuar." });
        }

        [HttpPost("ReenviarCodigoLogin")]
        public async Task<IActionResult> ReenviarCodigoLogin([FromBody] string correo)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo);
            if (usuario == null)
                return NotFound(new { mensaje = "El correo no está registrado." });

            var codigo = new Random().Next(100000, 999999).ToString();
            codigosLogin[correo] = new CodigoTemporal { Codigo = codigo, FechaExpiracion = DateTime.Now.AddMinutes(10) };

            var cuerpoHtml = CrearCorreoReenvioLogin(usuario.NombreCompleto, codigo);
            await EnviarCorreo(correo, "Código de verificación (reenvío)", cuerpoHtml);
            return Ok(new { mensaje = "Código reenviado correctamente." });
        }

        [HttpPost("LoginPaso2")]
        public async Task<IActionResult> LoginPaso2([FromBody] VerificacionCorreo modelo)
        {
            if (!codigosLogin.TryGetValue(modelo.Correo, out var temporal))
                return BadRequest(new { mensaje = "No se ha solicitado un código para este correo." });

            if (DateTime.Now > temporal.FechaExpiracion)
                return BadRequest(new { mensaje = "El código ha expirado." });

            if (modelo.Codigo != temporal.Codigo)
                return BadRequest(new { mensaje = "Código incorrecto." });

            codigosLogin.Remove(modelo.Correo);

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == modelo.Correo);
            if (usuario == null)
                return NotFound(new { mensaje = "Usuario no encontrado." });

            var token = GenerarToken(usuario);

            return Ok(new
            {
                token,
                idRol = usuario.IdRol,
                usuario = new
                {
                    usuario.NombreCompleto,
                    usuario.TipoDocumento,
                    usuario.NumDocumento,
                    usuario.Celular,
                    usuario.Departamento,
                    usuario.Ciudad,
                    usuario.Direccion
                }
            });
        }

        [HttpPost("RecuperarPaso1")]
        public async Task<IActionResult> RecuperarPaso1([FromBody] string correo)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo);
            if (usuario == null)
                return NotFound(new { mensaje = "El correo no está registrado." });

            var codigo = new Random().Next(100000, 999999).ToString();
            codigosRecuperacion[correo] = new CodigoTemporal { Codigo = codigo, FechaExpiracion = DateTime.Now.AddMinutes(10) };

            var cuerpoHtml = CrearCorreoRecuperacion(codigo);
            await EnviarCorreo(correo, "Recuperación de contraseña", cuerpoHtml);
            return Ok(new { mensaje = "Código de recuperación enviado al correo." });
        }

        [HttpPost("ReenviarCodigoRecuperacion")]
        public async Task<IActionResult> ReenviarCodigoRecuperacion([FromBody] string correo)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo);
            if (usuario == null)
                return NotFound(new { mensaje = "El correo no está registrado." });

            var codigo = new Random().Next(100000, 999999).ToString();
            codigosRecuperacion[correo] = new CodigoTemporal { Codigo = codigo, FechaExpiracion = DateTime.Now.AddMinutes(10) };

            var cuerpoHtml = CrearCorreoReenvioRecuperacion(codigo);
            await EnviarCorreo(correo, "Código de recuperación (reenvío)", cuerpoHtml);
            return Ok(new { mensaje = "Código reenviado correctamente." });
        }

        [HttpPost("RecuperarPaso2")]
        public async Task<IActionResult> RecuperarPaso2([FromBody] RecuperarRequest model)
        {
            if (!codigosRecuperacion.TryGetValue(model.Correo, out var temp))
                return BadRequest(new { mensaje = "No se ha solicitado un código para este correo." });

            if (temp.Codigo != model.Codigo)
                return BadRequest(new { mensaje = "Código incorrecto." });

            if (DateTime.Now > temp.FechaExpiracion)
                return BadRequest(new { mensaje = "El código ha expirado." });

            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == model.Correo);
            if (usuario == null)
                return NotFound(new { mensaje = "Usuario no encontrado." });

            usuario.Contrasena = model.NuevaContrasena;
            await _context.SaveChangesAsync();
            codigosRecuperacion.Remove(model.Correo);

            return Ok(new { mensaje = "Contraseña actualizada correctamente." });


        }

        [HttpPost("ValidarCodigoRecuperacion")]
        public IActionResult ValidarCodigoRecuperacion([FromBody] VerificacionCorreo model)
        {
            if (!codigosRecuperacion.TryGetValue(model.Correo, out var temp))
                return BadRequest(new { mensaje = "No se ha solicitado un código para este correo." });

            if (temp.Codigo != model.Codigo)
                return BadRequest(new { mensaje = "Código incorrecto." });

            if (DateTime.Now > temp.FechaExpiracion)
                return BadRequest(new { mensaje = "El código ha expirado." });

            return Ok(new { mensaje = "Código válido." });
        }


        private async Task EnviarCorreo(string destinatario, string asunto, string cuerpoHtml)
        {
            var correoEmisor = "angelrinconorozco11@gmail.com";
            var claveApp = "motw rndg ehiq gvso";

            var smtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(correoEmisor, claveApp),
                EnableSsl = true
            };

            var mensaje = new MailMessage
            {
                From = new MailAddress(correoEmisor),
                Subject = asunto,
                Body = cuerpoHtml,
                IsBodyHtml = true
            };

            mensaje.To.Add(destinatario);
            await smtp.SendMailAsync(mensaje);
        }

        private string GenerarToken(Usuario usuario)
        {
            var jwtKey = _config["Jwt:Key"];
            var jwtIssuer = _config["Jwt:Issuer"];
            var jwtAudience = _config["Jwt:Audience"];

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, usuario.NombreCompleto ?? ""),
                new Claim(ClaimTypes.Email, usuario.Correo ?? ""),
                new Claim("Id", usuario.IdUsuarios.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string CrearCorreoLogin(string nombre, string codigo) => $@"
<html>
  <body style='font-family: Arial, sans-serif; background-color: #f7f7f7; padding: 30px;'>
    <div style='max-width: 600px; margin: auto; background: white; border-radius: 8px; padding: 30px; box-shadow: 0 0 10px rgba(0,0,0,0.1);'>
      <div style='text-align: center;'>
        <img src='https://img.freepik.com/vector-premium/mensaje-correo-electronico-confirmacion-icono-envio-correo-electronico-correo-verificado-documento-marca-verificacion-sobre-correo_659151-1358.jpg'
             alt='Confirmación' style='width: 150px; margin-bottom: 20px;' />
        <h2 style='color: #2c3e50;'>¡Hola {nombre}!</h2>
      </div>
      <p style='font-size: 16px; color: #333;'>Bienvenido a nuestro sitio web, ingresa este código y explora nuestro emprendimiento:</p>
      <div style='font-size: 32px; font-weight: bold; color: white; background-color: #27ae60; padding: 15px; text-align: center; border-radius: 8px; margin: 20px 0;'>{codigo}</div>
      <p style='font-size: 14px; color: #666;'>Este código es válido por 10 minutos. Si no fuiste tú quien lo solicitó, ignora este mensaje.</p>
      <hr style='margin-top: 40px;' />
      <p style='font-size: 12px; color: #aaa;'>No respondas a este correo. Fue generado automáticamente.</p>
    </div>
  </body>
</html>";

        private string CrearCorreoReenvioLogin(string nombre, string codigo) => $@"
<html>
  <body style='font-family: Arial, sans-serif; background-color: #f7f7f7; padding: 30px;'>
    <div style='max-width: 600px; margin: auto; background: white; border-radius: 8px; padding: 30px; box-shadow: 0 0 10px rgba(0,0,0,0.1);'>
      <div style='text-align: center;'>
        <img src='https://img.freepik.com/vector-premium/mensaje-correo-electronico-confirmacion-icono-envio-correo-electronico-correo-verificado-documento-marca-verificacion-sobre-correo_659151-1358.jpg'
             alt='Confirmación' style='width: 150px; margin-bottom: 20px;' />
        <h2 style='color: #2c3e50;'>¡Hola {nombre}!</h2>
      </div>
      <p style='font-size: 16px; color: #333;'>Este es tu nuevo código de verificación:</p>
      <div style='font-size: 32px; font-weight: bold; color: white; background-color: #27ae60; padding: 15px; text-align: center; border-radius: 8px; margin: 20px 0;'>{codigo}</div>
      <p style='font-size: 14px; color: #666;'>Este código es válido por 10 minutos.</p>
    </div>
  </body>
</html>";

        private string CrearCorreoRecuperacion(string codigo) => $@"
<html>
  <body style='font-family: Arial, sans-serif; background-color: #f0f8f7; padding: 30px;'>
    <div style='max-width: 600px; margin: auto; background: white; border-radius: 12px; padding: 30px; box-shadow: 0 0 15px rgba(0,0,0,0.1);'>
      <div style='text-align: center;'>
        <img src='https://wwwp.ugc.edu.co/sede/bogota/pages/restablecer/app/imagenes/cambiar.png'
             alt='Recuperar contraseña' style='width: 120px; margin-bottom: 20px;' />
        <h2 style='color: #34495e;'>¿Olvidaste tu contraseña?</h2>
      </div>
      <p style='font-size: 16px; color: #2c3e50;'>¡No te preocupes! Este es tu código para recuperar el acceso:</p>
      <div style='font-size: 36px; font-weight: bold; color: white; background-color: #1abc9c; padding: 15px 0; text-align: center; border-radius: 8px; margin: 30px 0;'>{codigo}</div>
      <p style='font-size: 14px; color: #555;'>Este código es válido por 10 minutos.</p>
    </div>
  </body>
</html>";

        private string CrearCorreoReenvioRecuperacion(string codigo) => $@"
<html>
  <body style='font-family: Arial, sans-serif; background-color: #f0f8f7; padding: 30px;'>
    <div style='max-width: 600px; margin: auto; background: white; border-radius: 12px; padding: 30px; box-shadow: 0 0 15px rgba(0,0,0,0.1);'>
      <div style='text-align: center;'>
        <img src='https://wwwp.ugc.edu.co/sede/bogota/pages/restablecer/app/imagenes/cambiar.png'
             alt='Código reenviado' style='width: 120px; margin-bottom: 20px;' />
        <h2 style='color: #34495e;'>¡Aquí tienes tu nuevo código!</h2>
      </div>
      <p style='font-size: 16px; color: #2c3e50;'>Has solicitado un nuevo código para recuperar tu contraseña. Ingresa el siguiente código en la app:</p>
      <div style='font-size: 36px; font-weight: bold; color: white; background-color: #1abc9c; padding: 15px 0; text-align: center; border-radius: 8px; margin: 30px 0;'>{codigo}</div>
      <p style='font-size: 14px; color: #555;'>Este código es válido por <strong>10 minutos</strong>. Si no solicitaste esta recuperación, simplemente ignora este correo.</p>
      <hr style='margin-top: 40px; border: none; border-top: 1px solid #ddd;' />
      <p style='font-size: 12px; color: #aaa; text-align: center;'>Creart Niño - Email automático. No respondas a este mensaje.</p>
    </div>
  </body>
</html>";


        public class CodigoTemporal
        {
            public string Codigo { get; set; }
            public DateTime FechaExpiracion { get; set; }
        }

        public class RecuperarRequest
        {
            public string Correo { get; set; }
            public string Codigo { get; set; }
            public string NuevaContrasena { get; set; }
        }
    }
}
