using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OlivarBackend.Data;
using OlivarBackend.DTOs;
using OlivarBackend.Models;

namespace OlivarBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuponesController : ControllerBase
    {
        private readonly RestauranteDbContext _context;

        public CuponesController(RestauranteDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CuponDto>>> GetCupones()
        {
            var cupones = await _context.Cupones
                .Select(c => new CuponDto
                {
                    CuponId = c.CuponId,
                    Codigo = c.Codigo,
                    DescuentoPorcentaje = c.DescuentoPorcentaje,
                    FechaInicio = c.FechaInicio,
                    FechaFin = c.FechaFin,
                    Activo = c.Activo
                })
                .ToListAsync();

            return Ok(cupones);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CuponDto>> GetCupon(int id)
        {
            var c = await _context.Cupones.FindAsync(id);
            if (c == null)
                return NotFound();

            return new CuponDto
            {
                CuponId = c.CuponId,
                Codigo = c.Codigo,
                DescuentoPorcentaje = c.DescuentoPorcentaje,
                FechaInicio = c.FechaInicio,
                FechaFin = c.FechaFin,
                Activo = c.Activo
            };
        }

        [HttpPost]
        public async Task<ActionResult<CuponDto>> PostCupon(CuponDto dto)
        {
            var cupon = new Cupone
            {
                Codigo = dto.Codigo,
                DescuentoPorcentaje = dto.DescuentoPorcentaje,
                FechaInicio = dto.FechaInicio,
                FechaFin = dto.FechaFin,
                Activo = dto.Activo ?? true
            };

            _context.Cupones.Add(cupon);
            await _context.SaveChangesAsync();

            dto.CuponId = cupon.CuponId;
            return CreatedAtAction(nameof(GetCupon), new { id = cupon.CuponId }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCupon(int id, CuponDto dto)
        {
            if (id != dto.CuponId)
                return BadRequest();

            var cupon = await _context.Cupones.FindAsync(id);
            if (cupon == null)
                return NotFound();

            cupon.Codigo = dto.Codigo;
            cupon.DescuentoPorcentaje = dto.DescuentoPorcentaje;
            cupon.FechaInicio = dto.FechaInicio;
            cupon.FechaFin = dto.FechaFin;
            cupon.Activo = dto.Activo;

            _context.Entry(cupon).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCupon(int id)
        {
            var cupon = await _context.Cupones.FindAsync(id);
            if (cupon == null)
                return NotFound();

            _context.Cupones.Remove(cupon);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
