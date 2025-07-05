using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OlivarBackend.Data;
using OlivarBackend.DTOs;
using OlivarBackend.Models;

namespace OlivarBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturasController : ControllerBase
    {
        private readonly RestauranteDbContext _context;

        public FacturasController(RestauranteDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FacturaDto>>> GetFacturas()
        {
            var facturas = await _context.Facturas
                .Select(f => new FacturaDto
                {
                    FacturaId = f.FacturaId,
                    PedidoId = f.PedidoId,
                    RazonSocial = f.RazonSocial,
                    Ruc = f.Ruc,
                    DireccionFiscal = f.DireccionFiscal,
                    FechaEmision = f.FechaEmision,
                    MontoTotal = f.MontoTotal
                })
                .ToListAsync();

            return Ok(facturas);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FacturaDto>> GetFactura(int id)
        {
            var factura = await _context.Facturas
                .Where(f => f.FacturaId == id)
                .Select(f => new FacturaDto
                {
                    FacturaId = f.FacturaId,
                    PedidoId = f.PedidoId,
                    RazonSocial = f.RazonSocial,
                    Ruc = f.Ruc,
                    DireccionFiscal = f.DireccionFiscal,
                    FechaEmision = f.FechaEmision,
                    MontoTotal = f.MontoTotal
                })
                .FirstOrDefaultAsync();

            if (factura == null)
                return NotFound();

            return Ok(factura);
        }

        [HttpPost]
        public async Task<ActionResult<FacturaDto>> PostFactura(FacturaDto dto)
        {
            var factura = new Factura
            {
                PedidoId = dto.PedidoId,
                RazonSocial = dto.RazonSocial,
                Ruc = dto.Ruc,
                DireccionFiscal = dto.DireccionFiscal,
                FechaEmision = dto.FechaEmision ?? DateTime.Now,
                MontoTotal = dto.MontoTotal
            };

            _context.Facturas.Add(factura);
            await _context.SaveChangesAsync();

            dto.FacturaId = factura.FacturaId;
            return CreatedAtAction(nameof(GetFactura), new { id = dto.FacturaId }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutFactura(int id, FacturaDto dto)
        {
            if (id != dto.FacturaId)
                return BadRequest();

            var factura = await _context.Facturas.FindAsync(id);
            if (factura == null)
                return NotFound();

            factura.PedidoId = dto.PedidoId;
            factura.RazonSocial = dto.RazonSocial;
            factura.Ruc = dto.Ruc;
            factura.DireccionFiscal = dto.DireccionFiscal;
            factura.FechaEmision = dto.FechaEmision;
            factura.MontoTotal = dto.MontoTotal;

            _context.Entry(factura).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFactura(int id)
        {
            var factura = await _context.Facturas.FindAsync(id);
            if (factura == null)
                return NotFound();

            _context.Facturas.Remove(factura);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
