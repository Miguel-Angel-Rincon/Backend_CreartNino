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
            var correoEmisor = "creartnino@gmail.com";
            var claveApp = "nsul kpyv ujdk fkpn";

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
  <body style='font-family: Arial, sans-serif; background-color: #fff0f5; padding: 30px;'>
    <div style='max-width: 600px; margin: auto; background: white; border-radius: 16px; padding: 30px; box-shadow: 0 4px 20px rgba(0,0,0,0.08);'>
      
      <!-- Logo circular -->
      <div style='text-align: center; margin-bottom: 20px;'>
        <img src='https://res.cloudinary.com/ditcytztj/image/upload/v1759263193/logo.jpg_nxtres.jpg'
             alt='Confirmación'
             style='width: 120px; height: 120px; border-radius: 50%; object-fit: cover; box-shadow: 0 4px 10px rgba(0,0,0,0.1);' />
      </div>
      
      <!-- Título -->
      <h2 style='color: #34495e; text-align: center; margin-bottom: 10px;'>¡Hola {nombre}!</h2>
      <p style='font-size: 16px; color: #555; text-align: center; margin-bottom: 25px;'>
        Bienvenido a <b>CreartNino</b>. Ingresa este código y explora nuestro emprendimiento ✨
      </p>
      
      <!-- Código con fondo pastel rosado -->
      <div style='font-size: 32px; font-weight: bold; color: #fff; 
                  background: linear-gradient(135deg, #f8a5c2, #f78fb3); 
                  padding: 18px; text-align: center; border-radius: 12px; margin: 20px auto; max-width: 300px;'>
        {codigo}
      </div>
      
      <!-- Info -->
      <p style='font-size: 14px; color: #666; text-align: center;'>
        Este código es válido por <b>10 minutos</b>.  
        Si no fuiste tú quien lo solicitó, ignora este mensaje.
      </p>
      
      <!-- Línea divisoria -->
      <hr style='margin: 40px 0; border: none; border-top: 1px solid #eee;' />
      
      <!-- Footer -->
      <p style='font-size: 12px; color: #aaa; text-align: center;'>
        No respondas a este correo. Fue generado automáticamente.
      </p>
      <p style='font-size: 12px; color: #d6336c; text-align: center; font-weight: bold; margin-top: 10px;'>
        ⚠️ Este código es válido solo por 10 minutos ⚠️
      </p>
    </div>
  </body>
</html>";


        private string CrearCorreoReenvioLogin(string nombre, string codigo) => $@"
<html>
  <body style='font-family: Arial, sans-serif; background-color: #fff0f5; padding: 30px;'>
    <div style='max-width: 600px; margin: auto; background: white; border-radius: 16px; padding: 30px; box-shadow: 0 4px 20px rgba(0,0,0,0.08);'>
      
      <!-- Logo circular -->
      <div style='text-align: center; margin-bottom: 20px;'>
        <img src='https://res.cloudinary.com/ditcytztj/image/upload/v1759263193/logo.jpg_nxtres.jpg'
             alt='Confirmación'
             style='width: 120px; height: 120px; border-radius: 50%; object-fit: cover; box-shadow: 0 4px 10px rgba(0,0,0,0.1);' />
      </div>
      
      <!-- Título -->
      <h2 style='color: #34495e; text-align: center; margin-bottom: 10px;'>¡Hola {nombre}!</h2>
      <p style='font-size: 16px; color: #555; text-align: center; margin-bottom: 25px;'>
        Este es tu <b>nuevo código de verificación</b>.  
        Por favor ingrésalo para continuar con tu inicio de sesión ✨
      </p>
      
      <!-- Código con fondo pastel rosado -->
      <div style='font-size: 32px; font-weight: bold; color: #fff; 
                  background: linear-gradient(135deg, #f8a5c2, #f78fb3); 
                  padding: 18px; text-align: center; border-radius: 12px; margin: 20px auto; max-width: 300px;'>
        {codigo}
      </div>
      
      <!-- Info -->
      <p style='font-size: 14px; color: #666; text-align: center;'>
        Este código es válido por <b>10 minutos</b>.  
        Si no solicitaste este reenvío, ignora este mensaje.
      </p>
      
      <!-- Línea divisoria -->
      <hr style='margin: 40px 0; border: none; border-top: 1px solid #eee;' />
      
      <!-- Footer -->
      <p style='font-size: 12px; color: #aaa; text-align: center;'>
        No respondas a este correo. Fue generado automáticamente.
      </p>
      <p style='font-size: 12px; color: #d6336c; text-align: center; font-weight: bold; margin-top: 10px;'>
        ⚠️ Este código es válido solo por 10 minutos ⚠️
      </p>
    </div>
  </body>
</html>";


        private string CrearCorreoRecuperacion(string codigo) => $@"
<html>
  <body style='font-family: Arial, sans-serif; background-color: #fff0f5; padding: 30px;'>
    <div style='max-width: 600px; margin: auto; background: white; border-radius: 16px; padding: 30px; box-shadow: 0 4px 20px rgba(0,0,0,0.08);'>
      
      <!-- Logo circular -->
      <div style='text-align: center; margin-bottom: 20px;'>
        <img src='https://res.cloudinary.com/ditcytztj/image/upload/v1759263193/logo.jpg_nxtres.jpg'
             alt='Recuperar contraseña'
             style='width: 120px; height: 120px; border-radius: 50%; object-fit: cover; box-shadow: 0 4px 10px rgba(0,0,0,0.1);' />
      </div>
      
      <!-- Título -->
      <h2 style='color: #34495e; text-align: center; margin-bottom: 10px;'>¿Olvidaste tu contraseña?</h2>
      <p style='font-size: 16px; color: #555; text-align: center; margin-bottom: 25px;'>
        ¡No te preocupes! 💫 Ingresa el siguiente <b>código de recuperación</b> para restablecer tu acceso de forma segura.
      </p>
      
      <!-- Código con fondo pastel rosado -->
      <div style='font-size: 36px; font-weight: bold; color: #fff; 
                  background: linear-gradient(135deg, #f8a5c2, #f78fb3); 
                  padding: 20px; text-align: center; border-radius: 12px; margin: 30px auto; max-width: 300px;'>
        {codigo}
      </div>
      
      <!-- Info -->
      <p style='font-size: 14px; color: #666; text-align: center;'>
        Este código es válido por <b>10 minutos</b>.  
        Si no solicitaste la recuperación de contraseña, simplemente ignora este mensaje.
      </p>
      
      <!-- Línea divisoria -->
      <hr style='margin: 40px 0; border: none; border-top: 1px solid #eee;' />
      
      <!-- Footer -->
      <p style='font-size: 12px; color: #aaa; text-align: center;'>
        No respondas a este correo. Fue generado automáticamente.
      </p>
      <p style='font-size: 12px; color: #d6336c; text-align: center; font-weight: bold; margin-top: 10px;'>
        ⚠️ Este código es válido solo por 10 minutos ⚠️
      </p>
    </div>
  </body>
</html>";


        private string CrearCorreoReenvioRecuperacion(string codigo) => $@"
<html>
  <body style='font-family: Arial, sans-serif; background-color: #fff0f5; padding: 30px;'>
    <div style='max-width: 600px; margin: auto; background: white; border-radius: 16px; padding: 30px; box-shadow: 0 4px 20px rgba(0,0,0,0.08);'>
      
      <!-- Logo circular -->
      <div style='text-align: center; margin-bottom: 20px;'>
        <img src='https://res.cloudinary.com/ditcytztj/image/upload/v1759263193/logo.jpg_nxtres.jpg'
             alt='Código reenviado'
             style='width: 120px; height: 120px; border-radius: 50%; object-fit: cover; box-shadow: 0 4px 10px rgba(0,0,0,0.1);' />
      </div>
      
      <!-- Título -->
      <h2 style='color: #34495e; text-align: center; margin-bottom: 10px;'>¡Aquí tienes tu nuevo código!</h2>
      <p style='font-size: 16px; color: #555; text-align: center; margin-bottom: 25px;'>
        Has solicitado un <b>nuevo código</b> para recuperar tu contraseña.  
        Ingresa el siguiente código en la aplicación para continuar ✨
      </p>
      
      <!-- Código con fondo pastel rosado -->
      <div style='font-size: 36px; font-weight: bold; color: #fff; 
                  background: linear-gradient(135deg, #f8a5c2, #f78fb3); 
                  padding: 20px; text-align: center; border-radius: 12px; margin: 30px auto; max-width: 300px;'>
        {codigo}
      </div>
      
      <!-- Info -->
      <p style='font-size: 14px; color: #666; text-align: center;'>
        Este código es válido por <b>10 minutos</b>.  
        Si no solicitaste esta recuperación, simplemente ignora este correo.
      </p>
      
      <!-- Línea divisoria -->
      <hr style='margin: 40px 0; border: none; border-top: 1px solid #eee;' />
      
      <!-- Footer -->
      <p style='font-size: 12px; color: #aaa; text-align: center;'>
        CreartNino - Email automático. No respondas a este mensaje.
      </p>
      <p style='font-size: 12px; color: #d6336c; text-align: center; font-weight: bold; margin-top: 10px;'>
        ⚠️ Este código es válido solo por 10 minutos ⚠️
      </p>
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
//cambios realizados en el códigoo: