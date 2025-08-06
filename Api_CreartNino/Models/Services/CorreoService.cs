using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Api_CreartNino.Services
{
    public class CorreoService
    {
        private readonly string correoEmisor = "angelrinconorozco11@gmail.com"; // Tu correo
        private readonly string claveApp = "motw rndg ehiq gvso"; // Tu contraseña de app

        public async Task EnviarCorreoAsync(string destino, string asunto, string codigo)
        {
            var cuerpoHtml = $@"
<html>
  <body style='font-family: Arial, sans-serif; color: #333; background-color: #f4f4f4; padding: 30px;'>
    <div style='max-width: 600px; margin: auto; background: white; padding: 30px; border-radius: 10px; box-shadow: 0 4px 12px rgba(0,0,0,0.1);'>

      <div style='text-align: center;'>
        <img src='https://img.freepik.com/vector-premium/mensaje-correo-electronico-confirmacion-icono-envio-correo-electronico-correo-verificado-documento-marca-verificacion-sobre-correo_659151-1358.jpg' 
             alt='Confirmación de correo' 
             style='max-width: 100%; height: auto; border-radius: 10px; margin-bottom: 20px;' />
        <h2 style='color: #2c3e50;'>Código de Verificación</h2>
      </div>

      <p>Hola,</p>
      <p><strong>Bienvenido a nuestro sitio web, ingresa este código y explora nuestro emprendimiento.</strong></p>

      <div style='font-size: 36px; font-weight: bold; color: white; background-color: #27ae60; padding: 15px; text-align: center; border-radius: 8px; margin: 30px 0;'>
        {codigo}
      </div>

      <p>Este código es válido durante los próximos <strong>10 minutos</strong>. Por favor, no lo compartas con nadie.</p>

      <hr style='margin-top: 40px;'>
      <p style='font-size: 12px; color: #888;'>Este es un mensaje automático. Por favor, no respondas a este correo.</p>
    </div>
  </body>
</html>";

            var smtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(correoEmisor, claveApp),
                EnableSsl = true
            };

            var mensaje = new MailMessage(correoEmisor, destino, asunto, cuerpoHtml);
            mensaje.IsBodyHtml = true;

            await smtp.SendMailAsync(mensaje);
        }
    }
}