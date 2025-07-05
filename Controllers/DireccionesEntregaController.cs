using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OlivarBackend.Data;
using OlivarBackend.Models;

namespace OlivarBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DireccionesEntregaController : ControllerBase
    {
        private readonly RestauranteDbContext _context;

        public DireccionesEntregaController(RestauranteDbContext context)
        {
            _context = context;
        }

        // GET: api/DireccionesEntrega
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DireccionesEntrega>>> GetDirecciones()
        {
            return await _context.DireccionesEntregas.Include(d => d.Usuario).ToListAsync();
        }

        // GET: api/DireccionesEntrega/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DireccionesEntrega>> GetDireccion(int id)
        {
            var direccion = await _context.DireccionesEntregas.FindAsync(id);

            if (direccion == null)
                return NotFound();

            return direccion;
        }

        // POST: api/DireccionesEntrega
        [HttpPost]
        public async Task<ActionResult<DireccionesEntrega>> PostDireccion(DireccionesEntrega direccion)
        {
            _context.DireccionesEntregas.Add(direccion);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDireccion), new { id = direccion.DireccionId }, direccion);
        }

        // PUT: api/DireccionesEntrega/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDireccion(int id, DireccionesEntrega direccion)
        {
            if (id != direccion.DireccionId)
                return BadRequest();

            _context.Entry(direccion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DireccionesEntregaExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/DireccionesEntrega/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDireccion(int id)
        {
            var direccion = await _context.DireccionesEntregas.FindAsync(id);
            if (direccion == null)
                return NotFound();

            _context.DireccionesEntregas.Remove(direccion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DireccionesEntregaExists(int id)
        {
            return _context.DireccionesEntregas.Any(e => e.DireccionId == id);
        }
    }
}
