using Microsoft.AspNetCore.Mvc;
using System;

namespace Api_CreartNino.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilidadesController : ControllerBase
    {
        // ✅ Endpoint: GET api/Utilidades/FechaServidor
        [HttpGet("FechaServidor")]
        public IActionResult GetFechaServidor()
        {
            DateTime fechaServidor;

            try
            {
                // 🔹 Intentar obtener zona horaria de Windows (ej: Azure, IIS)
                var zona = TimeZoneInfo.FindSystemTimeZoneById("SA Pacific Standard Time");
                fechaServidor = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, zona);
            }
            catch (TimeZoneNotFoundException)
            {
                // 🔹 Si está en Linux (usa nombre IANA)
                var zona = TimeZoneInfo.FindSystemTimeZoneById("America/Bogota");
                fechaServidor = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, zona);
            }

            // 🔹 Retornar en formato ISO estándar
            return Ok(new { FechaServidor = fechaServidor });
        }

        
    }
}
