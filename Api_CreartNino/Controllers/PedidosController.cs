using Api_CreartNino.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Api_CreartNino.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly CreartNinoContext dbContext;
        public PedidosController(CreartNinoContext context)
        {
            dbContext = context;
        }

        // GET: api/estado
        [HttpGet]
        [Route("Lista")]

        public async Task<IActionResult> Get()
        {
            var pedidos = await dbContext.Pedidos.ToListAsync();
            return StatusCode(StatusCodes.Status200OK, pedidos);
        }

        // GET: estado/Obtener/5
        [HttpGet("Obtener/{id:int}")]
        public async Task<IActionResult> Obtener(int id)
        {
            var pedidos = await dbContext.Pedidos.FindAsync(id);
            if (pedidos == null)
            {
                return NotFound(new { mensaje = "Pedido no encontrado." });
            }
            return Ok(pedidos);
        }

        public class PedidoDTO
        {
            public int? IdCliente { get; set; }

            public string? MetodoPago { get; set; }

            public DateTime? FechaPedido { get; set; }  // ⬅️ Cambiado de DateOnly? a DateTime?

            public DateTime? FechaEntrega { get; set; } // ⬅️ Cambiado de DateOnly? a DateTime?

            public string? Descripcion { get; set; }

            public int? ValorInicial { get; set; }

            public int? ValorRestante { get; set; }

            public int? TotalPedido { get; set; }

            public string? ComprobantePago { get; set; }

            public int? IdEstado { get; set; }

            public List<DetallePedidoDTO> DetallePedidos { get; set; } = new();
        }

        public class DetallePedidoDTO
        {
            public int IdProducto { get; set; }

            public int Cantidad { get; set; }

            public int Subtotal { get; set; }
        }


        [HttpPost("Crear")]
        public async Task<IActionResult> Crear([FromBody] PedidoDTO dto)
        {
            if (dto == null)
                return BadRequest(new { mensaje = "Datos inválidos." });

            try
            {
                var pedido = new Pedido
                {
                    IdCliente = dto.IdCliente,
                    MetodoPago = dto.MetodoPago,
                    FechaPedido = dto.FechaPedido,         // ✅ Asignación directa
                    FechaEntrega = dto.FechaEntrega,       // ✅ Asignación directa
                    Descripcion = dto.Descripcion,
                    ValorInicial = dto.ValorInicial,
                    ValorRestante = dto.ValorRestante,
                    TotalPedido = dto.TotalPedido,
                    ComprobantePago = dto.ComprobantePago,
                    IdEstado = dto.IdEstado
                };

                await dbContext.Pedidos.AddAsync(pedido);
                await dbContext.SaveChangesAsync();

                foreach (var det in dto.DetallePedidos)
                {
                    var nuevoDet = new DetallePedido
                    {
                        IdPedido = pedido.IdPedido,
                        IdProducto = det.IdProducto,
                        Cantidad = det.Cantidad,
                        Subtotal = det.Subtotal
                    };
                    dbContext.DetallePedidos.Add(nuevoDet);
                }

                await dbContext.SaveChangesAsync();

                return Ok(new { mensaje = "✅ Pedido creado correctamente.", pedido.IdPedido });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error interno al crear pedido: {ex}");
                return StatusCode(500, new { mensaje = "Error interno", detalle = ex.Message });
            }
        }



        // PUT: Proveedores/Actualizar/5
        [HttpPut]
        [Route("Actualizar/{id:int}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] Pedido objeto)
        {
            if (id != objeto.IdPedido)
            {
                return BadRequest("El ID en la URL no coincide con el ID del objeto.");
            }

            dbContext.Pedidos.Update(objeto);
            await dbContext.SaveChangesAsync();
            return Ok(new { mensaje = "Pedido Actualizado correctamente" });
        }

        [HttpDelete("Eliminar/{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var pedidos = await dbContext.Pedidos.FindAsync(id);
            if (pedidos == null)
            {
                return NotFound(new { mensaje = "Pedido no encontrado." });
            }

            try
            {
                dbContext.Pedidos.Remove(pedidos);
                await dbContext.SaveChangesAsync();
                return Ok(new { mensaje = "Pedido eliminado correctamente." });
            }
            catch (DbUpdateException ex)
            {
                // Aquí puedes verificar más a fondo si la excepción tiene que ver con claves foráneas.
                return Conflict(new { mensaje = "No se puede eliminar el Pedido porque está asociada a una o mas Producciones." });
            }
        }
    }
}
