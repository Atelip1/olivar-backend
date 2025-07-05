using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OlivarBackend.Data;
using OlivarBackend.Models;

namespace OlivarBackend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RolesController : ControllerBase
{
    private readonly RestauranteDbContext _context;

    public RolesController(RestauranteDbContext context)
    {
        _context = context;
    }

    // GET: api/Roles
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
    {
        var roles = await _context.Roles
                                  .Include(r => r.RolPermisos)
                                  .ThenInclude(rp => rp.Permiso)
                                  .ToListAsync();

        return Ok(roles);
    }

    // GET: api/Roles/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Role>> GetRol(int id)
    {
        var rol = await _context.Roles
                                .Include(r => r.RolPermisos)
                                .ThenInclude(rp => rp.Permiso)
                                .FirstOrDefaultAsync(r => r.RolId == id);

        if (rol == null)
            return NotFound(new { mensaje = "Rol no encontrado" });

        return Ok(rol);
    }

    // POST: api/Roles
    [HttpPost]
    public async Task<ActionResult<Role>> CrearRol(Role rol)
    {
        _context.Roles.Add(rol);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetRol), new { id = rol.RolId }, rol);
    }

    // POST: api/Roles/asignar-permisos
    [HttpPost("asignar-permisos")]
    public async Task<IActionResult> AsignarPermisos([FromBody] AsignarPermisosDTO dto)
    {
        var rol = await _context.Roles.FindAsync(dto.RolId);
        if (rol == null)
            return NotFound(new { mensaje = "Rol no encontrado" });

        var permisosExistentes = await _context.RolPermisos
            .Where(rp => rp.RolId == dto.RolId)
            .ToListAsync();

        _context.RolPermisos.RemoveRange(permisosExistentes);

        var nuevosPermisos = dto.PermisosIds.Select(id => new RolPermiso
        {
            RolId = dto.RolId,
            PermisoId = id
        });

        await _context.RolPermisos.AddRangeAsync(nuevosPermisos);
        await _context.SaveChangesAsync();

        return Ok(new { mensaje = "Permisos asignados correctamente." });
    }

    // POST: api/Roles/remover-permisos
    [HttpPost("remover-permisos")]
    public async Task<IActionResult> RemoverPermisos([FromBody] AsignarPermisosDTO dto)
    {
        var permisosAEliminar = await _context.RolPermisos
            .Where(rp => rp.RolId == dto.RolId && dto.PermisosIds.Contains(rp.PermisoId))
            .ToListAsync();

        if (!permisosAEliminar.Any())
            return NotFound(new { mensaje = "No se encontraron permisos para eliminar." });

        _context.RolPermisos.RemoveRange(permisosAEliminar);
        await _context.SaveChangesAsync();

        return Ok(new { mensaje = "Permisos eliminados correctamente." });
    }
}

// DTO para asignar o remover permisos
public class AsignarPermisosDTO
{
    public int RolId { get; set; }
    public List<int> PermisosIds { get; set; } = new();
}
