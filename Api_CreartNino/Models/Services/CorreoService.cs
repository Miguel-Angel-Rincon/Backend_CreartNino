using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Api_CreartNino.Services
{
    public class CorreoService
    {
        private readonly string correoEmisor = "creartnino@gmail.com"; // Tu correo
        private readonly string claveApp = "nsul kpyv ujdk fkpn"; // Tu contraseña de app

        public async Task EnviarCorreoAsync(string destino, string asunto, string codigo)
        {
            var cuerpoHtml = $@"
<html>
  <body style='font-family: Arial, sans-serif; color: #444; background-color: #fce4ec; padding: 30px;'>
    <div style='max-width: 600px; margin: auto; background: white; padding: 30px; border-radius: 12px; box-shadow: 0 4px 12px rgba(0,0,0,0.1);'>

      <div style='text-align: center;'>
        <img src='https://res.cloudinary.com/ditcytztj/image/upload/v1759263193/logo.jpg_nxtres.jpg' 
             alt='Confirmación de correo' 
             style='max-width: 120px; height: auto; border-radius: 10px; margin-bottom: 20px;' />
        <h2 style='color: #d81b60;'>Código de Verificación</h2>
      </div>

      <p>Hola,</p>
      <p><strong>Bienvenido a nuestro sitio web 💖, ingresa este código y explora nuestro emprendimiento.</strong></p>

      <div style='font-size: 36px; font-weight: bold; color: white; background-color: #ec407a; padding: 15px; text-align: center; border-radius: 8px; margin: 30px 0; letter-spacing: 5px;'>
        {codigo}
      </div>

      <p>Este código es válido durante los próximos <strong>10 minutos</strong>. Por favor, no lo compartas con nadie.</p>

      <hr style='margin: 40px 0; border: none; border-top: 1px solid #f8bbd0;'>

      <footer style='text-align: center; font-size: 12px; color: #ad1457;'>
        🌸 Este código es válido únicamente para tu verificación.  
        <br/>Gracias por confiar en nosotros 💕
      </footer>
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