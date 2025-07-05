using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OlivarBackend.Data;
using OlivarBackend.DTOs;
using OlivarBackend.Models;

namespace OlivarBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservasController : ControllerBase
    {
        private readonly RestauranteDbContext _context;

        public ReservasController(RestauranteDbContext context)
        {
            _context = context;
        }

        // GET: api/Reservas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservaDto>>> GetReservas()
        {
            var reservas = await _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Sucursal)
                .Select(r => new ReservaDto
                {
                    ReservaId = r.ReservaId,
                    UsuarioId = r.UsuarioId,
                    Fecha = r.Fecha,
                    Hora = r.Hora,
                    CantidadPersonas = r.CantidadPersonas,
                    Observaciones = r.Observaciones,
                    Estado = r.Estado,
                    FechaRegistro = r.FechaRegistro,
                    SucursalId = r.SucursalId,
                    NombreUsuario = r.Usuario.Nombre,
                    NombreSucursal = r.Sucursal != null ? r.Sucursal.Nombre : null
                })
                .ToListAsync();

            return Ok(reservas);
        }

        // GET: api/Reservas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ReservaDto>> GetReserva(int id)
        {
            var reserva = await _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Sucursal)
                .Where(r => r.ReservaId == id)
                .Select(r => new ReservaDto
                {
                    ReservaId = r.ReservaId,
                    UsuarioId = r.UsuarioId,
                    Fecha = r.Fecha,
                    Hora = r.Hora,
                    CantidadPersonas = r.CantidadPersonas,
                    Observaciones = r.Observaciones,
                    Estado = r.Estado,
                    FechaRegistro = r.FechaRegistro,
                    SucursalId = r.SucursalId,
                    NombreUsuario = r.Usuario.Nombre,
                    NombreSucursal = r.Sucursal != null ? r.Sucursal.Nombre : null
                })
                .FirstOrDefaultAsync();

            if (reserva == null)
                return NotFound();

            return Ok(reserva);
        }

        // POST: api/Reservas
        [HttpPost]
        public async Task<ActionResult<ReservaDto>> PostReserva(ReservaDto dto)
        {
            var reserva = new Reserva
            {
                UsuarioId = dto.UsuarioId,
                Fecha = dto.Fecha,
                Hora = dto.Hora,
                CantidadPersonas = dto.CantidadPersonas,
                Observaciones = dto.Observaciones,
                Estado = dto.Estado ?? "Pendiente",
                FechaRegistro = DateTime.Now,
                SucursalId = dto.SucursalId
            };

            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            dto.ReservaId = reserva.ReservaId;
            return CreatedAtAction(nameof(GetReserva), new { id = reserva.ReservaId }, dto);
        }

        // PUT: api/Reservas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReserva(int id, ReservaDto dto)
        {
            if (id != dto.ReservaId)
                return BadRequest();

            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
                return NotFound();

            reserva.Fecha = dto.Fecha;
            reserva.Hora = dto.Hora;
            reserva.CantidadPersonas = dto.CantidadPersonas;
            reserva.Observaciones = dto.Observaciones;
            reserva.Estado = dto.Estado;
            reserva.SucursalId = dto.SucursalId;

            _context.Entry(reserva).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Reservas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReserva(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
                return NotFound();

            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
