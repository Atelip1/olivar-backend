using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OlivarBackend.Data;
using OlivarBackend.Models;

namespace OlivarBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ConfiguracionController : ControllerBase
{
    private readonly RestauranteDbContext _context;

    public ConfiguracionController(RestauranteDbContext context)
    {
        _context = context;
    }

    // GET: api/Configuracion
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Configuracion>>> GetConfiguracion()
    {
        return await _context.Configuracions.ToListAsync();
    }

    // GET: api/Configuracion/clave
    [HttpGet("{clave}")]
    public async Task<ActionResult<Configuracion>> GetConfiguracion(string clave)
    {
        var config = await _context.Configuracions.FindAsync(clave);

        if (config == null)
            return NotFound();

        return config;
    }

    // POST: api/Configuracion
    [HttpPost]
    public async Task<ActionResult<Configuracion>> PostConfiguracion(Configuracion config)
    {
        _context.Configuracions.Add(config);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetConfiguracion), new { clave = config.Clave }, config);
    }

    // PUT: api/Configuracion/clave
    [HttpPut("{clave}")]
    public async Task<IActionResult> PutConfiguracion(string clave, Configuracion config)
    {
        if (clave != config.Clave)
            return BadRequest("La clave de la configuración no coincide.");

        _context.Entry(config).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Configuracions.Any(e => e.Clave == clave))
                return NotFound();
            else
                throw;
        }

        return NoContent();
    }

    // DELETE: api/Configuracion/clave
    [HttpDelete("{clave}")]
    public async Task<IActionResult> DeleteConfiguracion(string clave)
    {
        var config = await _context.Configuracions.FindAsync(clave);
        if (config == null)
            return NotFound();

        _context.Configuracions.Remove(config);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
