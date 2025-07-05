using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OlivarBackend.Data;
using OlivarBackend.DTOs;
using OlivarBackend.Models;

namespace OlivarBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagosController : ControllerBase
    {
        private readonly RestauranteDbContext _context;

        public PagosController(RestauranteDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PagoDto>>> GetPagos()
        {
            var pagos = await _context.Pagos
                .Select(p => new PagoDto
                {
                    PagoId = p.PagoId,
                    PedidoId = p.PedidoId,
                    Monto = p.Monto,
                    Metodo = p.Metodo,
                    FechaPago = p.FechaPago
                })
                .ToListAsync();

            return Ok(pagos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PagoDto>> GetPago(int id)
        {
            var pago = await _context.Pagos
                .Where(p => p.PagoId == id)
                .Select(p => new PagoDto
                {
                    PagoId = p.PagoId,
                    PedidoId = p.PedidoId,
                    Monto = p.Monto,
                    Metodo = p.Metodo,
                    FechaPago = p.FechaPago
                })
                .FirstOrDefaultAsync();

            if (pago == null)
                return NotFound();

            return Ok(pago);
        }

        [HttpPost]
        public async Task<ActionResult<PagoDto>> PostPago(PagoDto dto)
        {
            var pago = new Pago
            {
                PedidoId = dto.PedidoId,
                Monto = dto.Monto,
                Metodo = dto.Metodo,
                FechaPago = dto.FechaPago ?? DateTime.Now
            };

            _context.Pagos.Add(pago);
            await _context.SaveChangesAsync();

            dto.PagoId = pago.PagoId;
            return CreatedAtAction(nameof(GetPago), new { id = pago.PagoId }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPago(int id, PagoDto dto)
        {
            if (id != dto.PagoId)
                return BadRequest();

            var pago = await _context.Pagos.FindAsync(id);
            if (pago == null)
                return NotFound();

            pago.PedidoId = dto.PedidoId;
            pago.Monto = dto.Monto;
            pago.Metodo = dto.Metodo;
            pago.FechaPago = dto.FechaPago;

            _context.Entry(pago).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePago(int id)
        {
            var pago = await _context.Pagos.FindAsync(id);
            if (pago == null)
                return NotFound();

            _context.Pagos.Remove(pago);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
