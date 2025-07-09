using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OlivarBackend.Data;
using OlivarBackend.Dtos;
using OlivarBackend.Models;

namespace OlivarBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificacionesController : ControllerBase
    {
        private readonly RestauranteDbContext _context;

        public NotificacionesController(RestauranteDbContext context)
        {
            _context = context;
        }

        // GET: api/Notificaciones
        // Trae todas las notificaciones (útil para pruebas o admin)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NotificacionDto>>> GetNotificaciones()
        {
            var notificaciones = await _context.Notificaciones
                .Select(n => new NotificacionDto
                {
                    NotificacionId = n.NotificacionId,
                    UsuarioId = n.UsuarioId,
                    Titulo = n.Titulo,
                    Mensaje = n.Mensaje,
                    FechaEnvio = n.FechaEnvio,
                    Leida = n.Leida
                })
                .ToListAsync();

            return Ok(notificaciones);
        }

        //GET: api/Notificaciones/usuario/5
        // Trae las notificaciones del usuario + globales (UsuarioId == null)
        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<NotificacionDto>>> GetNotificacionesPorUsuario(int usuarioId)
        {
            var notificaciones = await _context.Notificaciones
                .Where(n => n.UsuarioId == usuarioId || n.UsuarioId == null)
                .OrderByDescending(n => n.FechaEnvio)
                .Select(n => new NotificacionDto
                {
                    NotificacionId = n.NotificacionId,
                    UsuarioId = n.UsuarioId,
                    Titulo = n.Titulo,
                    Mensaje = n.Mensaje,
                    FechaEnvio = n.FechaEnvio,
                    Leida = n.Leida
                })
                .ToListAsync();

            return Ok(notificaciones);
        }

        // GET: api/Notificaciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NotificacionDto>> GetNotificacion(int id)
        {
            var n = await _context.Notificaciones.FindAsync(id);
            if (n == null)
                return NotFound();

            var dto = new NotificacionDto
            {
                NotificacionId = n.NotificacionId,
                UsuarioId = n.UsuarioId,
                Titulo = n.Titulo,
                Mensaje = n.Mensaje,
                FechaEnvio = n.FechaEnvio,
                Leida = n.Leida
            };

            return Ok(dto);
        }

        // POST: api/Notificaciones
        [HttpPost]
        public async Task<ActionResult<NotificacionDto>> PostNotificacion(NotificacionCreateDto dto)
        {
            var notificacion = new Notificacione
            {
                UsuarioId = dto.UsuarioId,  
                Titulo = dto.Titulo,
                Mensaje = dto.Mensaje,
                FechaEnvio = DateTime.Now,
                Leida = false
            };

            _context.Notificaciones.Add(notificacion);
            await _context.SaveChangesAsync();

            var resultDto = new NotificacionDto
            {
                NotificacionId = notificacion.NotificacionId,
                UsuarioId = notificacion.UsuarioId,
                Titulo = notificacion.Titulo,
                Mensaje = notificacion.Mensaje,
                FechaEnvio = notificacion.FechaEnvio,
                Leida = notificacion.Leida
            };

            return CreatedAtAction(nameof(GetNotificacion), new { id = notificacion.NotificacionId }, resultDto);
        }

        //  PUT: api/Notificaciones/5/leer
        [HttpPut("{id}/leer")]
        public async Task<IActionResult> MarcarComoLeida(int id)
        {
            var n = await _context.Notificaciones.FindAsync(id);
            if (n == null)
                return NotFound();

            n.Leida = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Notificaciones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotificacion(int id)
        {
            var n = await _context.Notificaciones.FindAsync(id);
            if (n == null)
                return NotFound();

            _context.Notificaciones.Remove(n);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
