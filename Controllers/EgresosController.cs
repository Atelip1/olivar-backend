using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OlivarBackend.Data;
using OlivarBackend.Models;

namespace OlivarBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EgresosController : ControllerBase
    {
        private readonly RestauranteDbContext _context;

        public EgresosController(RestauranteDbContext context)
        {
            _context = context;
        }

        // GET: api/Egresos/dia/2025-07-08
        [HttpGet("dia/{fecha}")]
        public async Task<IActionResult> GetEgresosPorDia(DateTime fecha)
        {
            var egresos = await _context.Egresos
                .Where(e => e.Fecha.Date == fecha.Date)
                .ToListAsync();

            return Ok(egresos);
        }

        // POST: api/Egresos
        [HttpPost]
        public async Task<IActionResult> CrearEgreso([FromBody] Egreso egreso)
        {
            if (egreso == null)
                return BadRequest();

            _context.Egresos.Add(egreso);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEgresosPorDia), new { fecha = egreso.Fecha }, egreso);
        }
    }
}
