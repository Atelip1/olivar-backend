using Microsoft.AspNetCore.Mvc;
using OlivarBackend.DTOs;
using OlivarBackend.Services;

namespace OlivarBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermisosController : ControllerBase
    {
        private readonly IPermisoService _permisoService;

        public PermisosController(IPermisoService permisoService)
        {
            _permisoService = permisoService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PermisoDTO>>> Get()
        {
            return Ok(await _permisoService.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PermisoDTO>> Get(int id)
        {
            var permiso = await _permisoService.GetByIdAsync(id);
            if (permiso == null) return NotFound();
            return Ok(permiso);
        }

        [HttpPost]
        public async Task<ActionResult<PermisoDTO>> Post(PermisoDTO dto)
        {
            var created = await _permisoService.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created.PermisoId }, created);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, PermisoDTO dto)
        {
            var result = await _permisoService.UpdateAsync(id, dto);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _permisoService.DeleteAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}
