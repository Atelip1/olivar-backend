using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OlivarBackend.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OlivarBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportesController : ControllerBase
    {
        private readonly RestauranteDbContext _context;

        public ReportesController(RestauranteDbContext context)
        {
            _context = context;
        }

        // DTO de salida
        public class IngresoDiarioDTO
        {
            public DateTime Fecha { get; set; }
            public int NumeroPedidos { get; set; }
            public decimal TotalIngresos { get; set; }
        }

        // GET: api/Reportes/ingresos-dia/2025-07-08
        [HttpGet("ingresos-dia/{fecha}")]
        public async Task<ActionResult<IngresoDiarioDTO>> ObtenerIngresosPorDia(DateTime fecha)
        {
            var pedidos = await _context.Pedidos
                .Where(p => p.Fecha.HasValue && p.Fecha.Value.Date == fecha.Date && p.Estado == "Completado")

                .ToListAsync();

            var total = pedidos.Sum(p => p.Total);

            var resultado = new IngresoDiarioDTO
            {
                Fecha = fecha,
                NumeroPedidos = pedidos.Count,
                TotalIngresos = total
            };

            return Ok(resultado);
        }
        [HttpGet("pedidos-dia")]
        public IActionResult PedidosDelDia([FromQuery] DateTime fecha, [FromQuery] string estado = null)
        {
            var inicio = fecha.Date;
            var fin = inicio.AddDays(1);

            var pedidos = _context.Pedidos
                .Where(p => p.Fecha >= inicio && p.Fecha < fin);

            if (!string.IsNullOrEmpty(estado))
            {
                pedidos = pedidos.Where(p => p.Estado == estado);
            }

            var resultado = pedidos
                .Select(p => new
                {
                    PedidoId = p.PedidoId,           
                    UsuarioId = p.UsuarioId,
                    Total = p.Total,
                    Fecha = p.Fecha,
                    MetodoPago = p.MetodoPago,
                    MetodoEntrega = p.MetodoEntrega,
                    Estado = p.Estado
                })

                .ToList();

            return Ok(resultado);
        }

    }
}
