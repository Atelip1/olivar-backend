using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OlivarBackend.Data;
using OlivarBackend.DTOs;
using OlivarBackend.Models;

namespace OlivarBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistorialEstadoPedidosController : ControllerBase
    {
        private readonly RestauranteDbContext _context;

        public HistorialEstadoPedidosController(RestauranteDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HistorialEstadoPedidoDto>>> GetHistorial()
        {
            var historial = await _context.HistorialEstadoPedidos
                .Select(h => new HistorialEstadoPedidoDto
                {
                    EstadoId = h.EstadoId,
                    PedidoId = h.PedidoId,
                    Estado = h.Estado,
                    FechaHora = h.FechaHora
                })
                .ToListAsync();

            return Ok(historial);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<HistorialEstadoPedidoDto>> GetHistorialById(int id)
        {
            var estado = await _context.HistorialEstadoPedidos
                .Where(h => h.EstadoId == id)
                .Select(h => new HistorialEstadoPedidoDto
                {
                    EstadoId = h.EstadoId,
                    PedidoId = h.PedidoId,
                    Estado = h.Estado,
                    FechaHora = h.FechaHora
                })
                .FirstOrDefaultAsync();

            if (estado == null)
                return NotFound();

            return Ok(estado);
        }

        [HttpPost]
        public async Task<ActionResult<HistorialEstadoPedidoDto>> PostHistorial(HistorialEstadoPedidoDto dto)
        {
            var historial = new HistorialEstadoPedido
            {
                PedidoId = dto.PedidoId,
                Estado = dto.Estado,
                FechaHora = dto.FechaHora ?? DateTime.Now
            };

            _context.HistorialEstadoPedidos.Add(historial);
            await _context.SaveChangesAsync();

            dto.EstadoId = historial.EstadoId;
            return CreatedAtAction(nameof(GetHistorialById), new { id = dto.EstadoId }, dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHistorial(int id)
        {
            var historial = await _context.HistorialEstadoPedidos.FindAsync(id);
            if (historial == null)
                return NotFound();

            _context.HistorialEstadoPedidos.Remove(historial);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
