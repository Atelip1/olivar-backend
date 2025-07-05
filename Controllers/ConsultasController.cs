using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OlivarBackend.Data;
using OlivarBackend.Models;

namespace OlivarBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConsultasController : ControllerBase
    {
        private readonly RestauranteDbContext _context;

        public ConsultasController(RestauranteDbContext context)
        {
            _context = context;
        }

        // GET: api/Consultas -> listar todas las consultas (puedes filtrar por usuario con query)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConsultaDetailDto>>> GetConsultas([FromQuery] int? usuarioId)
        {
            var query = _context.Consultas.AsQueryable();

            if (usuarioId.HasValue)
                query = query.Where(c => c.UsuarioId == usuarioId.Value);

            var consultas = await query
                .Select(c => new ConsultaDetailDto
                {
                    ConsultaId = c.ConsultaId,
                    UsuarioId = c.UsuarioId,
                    Asunto = c.Asunto,
                    Mensaje = c.Mensaje,
                    Estado = c.Estado,
                    Fecha = c.Fecha
                })
                .ToListAsync();

            return Ok(consultas);
        }

        // GET: api/Consultas/5 -> obtener una consulta por id
        [HttpGet("{id}")]
        public async Task<ActionResult<ConsultaDetailDto>> GetConsulta(int id)
        {
            var consulta = await _context.Consultas
                .Where(c => c.ConsultaId == id)
                .Select(c => new ConsultaDetailDto
                {
                    ConsultaId = c.ConsultaId,
                    UsuarioId = c.UsuarioId,
                    Asunto = c.Asunto,
                    Mensaje = c.Mensaje,
                    Estado = c.Estado,
                    Fecha = c.Fecha
                })
                .FirstOrDefaultAsync();

            if (consulta == null)
                return NotFound();

            return Ok(consulta);
        }

        // POST: api/Consultas -> crear una consulta
        [HttpPost]
        public async Task<ActionResult<ConsultaDetailDto>> PostConsulta(ConsultaDto dto)
        {
            var consulta = new Consulta
            {
                UsuarioId = dto.UsuarioId,
                Asunto = dto.Asunto,
                Mensaje = dto.Mensaje,
                Estado = dto.Estado ?? "Pendiente",
                Fecha = DateTime.Now
            };

            _context.Consultas.Add(consulta);
            await _context.SaveChangesAsync();

            var result = new ConsultaDetailDto
            {
                ConsultaId = consulta.ConsultaId,
                UsuarioId = consulta.UsuarioId,
                Asunto = consulta.Asunto,
                Mensaje = consulta.Mensaje,
                Estado = consulta.Estado,
                Fecha = consulta.Fecha
            };

            return CreatedAtAction(nameof(GetConsulta), new { id = consulta.ConsultaId }, result);
        }

        // PUT: api/Consultas/5 -> actualizar una consulta (por ejemplo estado o mensaje)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConsulta(int id, ConsultaDto dto)
        {
            var consulta = await _context.Consultas.FindAsync(id);
            if (consulta == null)
                return NotFound();

            consulta.Asunto = dto.Asunto ?? consulta.Asunto;
            consulta.Mensaje = dto.Mensaje ?? consulta.Mensaje;
            consulta.Estado = dto.Estado ?? consulta.Estado;

            _context.Entry(consulta).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Consultas/5 -> eliminar una consulta
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConsulta(int id)
        {
            var consulta = await _context.Consultas.FindAsync(id);
            if (consulta == null)
                return NotFound();

            _context.Consultas.Remove(consulta);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
