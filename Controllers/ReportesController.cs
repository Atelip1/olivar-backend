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
    }
}
