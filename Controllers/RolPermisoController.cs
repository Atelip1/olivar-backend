using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OlivarBackend.Data;
using OlivarBackend.Models;

namespace OlivarBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolPermisoController : ControllerBase
{
    private readonly RestauranteDbContext _context;

    public RolPermisoController(RestauranteDbContext context)
    {
        _context = context;
    }

    // GET: api/RolPermiso
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RolPermiso>>> GetRolPermisos()
    {
        return await _context.RolPermisos
            .Include(rp => rp.Rol)
            .Include(rp => rp.Permiso)
            .ToListAsync();
    }

    // GET: api/RolPermiso/1/2
    [HttpGet("{rolId}/{permisoId}")]
    public async Task<ActionResult<RolPermiso>> GetRolPermiso(int rolId, int permisoId)
    {
        var rolPermiso = await _context.RolPermisos
            .Include(rp => rp.Rol)
            .Include(rp => rp.Permiso)
            .FirstOrDefaultAsync(rp => rp.RolId == rolId && rp.PermisoId == permisoId);

        if (rolPermiso == null)
            return NotFound(new { mensaje = "No se encontró la relación Rol-Permiso" });

        return rolPermiso;
    }

    // POST: api/RolPermiso
    [HttpPost]
    public async Task<ActionResult<RolPermiso>> CrearRolPermiso(RolPermiso dto)
    {
        // Verifica si ya existe
        bool yaExiste = await _context.RolPermisos
            .AnyAsync(rp => rp.RolId == dto.RolId && rp.PermisoId == dto.PermisoId);

        if (yaExiste)
            return Conflict(new { mensaje = "Ya existe esta relación Rol-Permiso" });

        _context.RolPermisos.Add(dto);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetRolPermiso), new { rolId = dto.RolId, permisoId = dto.PermisoId }, dto);
    }

    // DELETE: api/RolPermiso/1/2
    [HttpDelete("{rolId}/{permisoId}")]
    public async Task<IActionResult> EliminarRolPermiso(int rolId, int permisoId)
    {
        var rolPermiso = await _context.RolPermisos
            .FirstOrDefaultAsync(rp => rp.RolId == rolId && rp.PermisoId == permisoId);

        if (rolPermiso == null)
            return NotFound(new { mensaje = "Relación no encontrada" });

        _context.RolPermisos.Remove(rolPermiso);
        await _context.SaveChangesAsync();

        return Ok(new { mensaje = "Relación eliminada correctamente" });
    }
}
