using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OlivarBackend.Data;
using OlivarBackend.Models;

namespace OlivarBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LogsSistemaController : ControllerBase
{
    private readonly RestauranteDbContext _context;

    public LogsSistemaController(RestauranteDbContext context)
    {
        _context = context;
    }

    // GET: api/LogsSistema
    [HttpGet]
    public async Task<ActionResult<IEnumerable<LogsSistema>>> GetLogs()
    {
        return await _context.LogsSistemas
            .Include(l => l.Usuario)
            .OrderByDescending(l => l.Fecha)
            .ToListAsync();
    }

    // GET: api/LogsSistema/5
    [HttpGet("{id}")]
    public async Task<ActionResult<LogsSistema>> GetLog(int id)
    {
        var log = await _context.LogsSistemas
            .Include(l => l.Usuario)
            .FirstOrDefaultAsync(l => l.LogId == id);

        if (log == null)
        {
            return NotFound();
        }

        return log;
    }

    // POST: api/LogsSistema
    [HttpPost]
    public async Task<ActionResult<LogsSistema>> CrearLog([FromBody] CrearLogDto dto)
    {
        var log = new LogsSistema
        {
            UsuarioId = dto.UsuarioId,
            Accion = dto.Accion,
            TablaAfectada = dto.TablaAfectada,
            Fecha = DateTime.Now
        };

        _context.LogsSistemas.Add(log);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetLog), new { id = log.LogId }, log);
    }

    // DELETE: api/LogsSistema/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarLog(int id)
    {
        var log = await _context.LogsSistemas.FindAsync(id);
        if (log == null)
        {
            return NotFound();
        }

        _context.LogsSistemas.Remove(log);
        await _context.SaveChangesAsync();

        return Ok(new { mensaje = "Log eliminado correctamente" });
    }
}
