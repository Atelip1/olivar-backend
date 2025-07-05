using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OlivarBackend.Data;
using OlivarBackend.DTOs;
using OlivarBackend.Models;

namespace OlivarBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComentariosProductoController : ControllerBase
    {
        private readonly RestauranteDbContext _context;

        public ComentariosProductoController(RestauranteDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComentariosProductoDto>>> GetComentarios()
        {
            return await _context.ComentariosProductos
                .Select(c => new ComentariosProductoDto
                {
                    ComentarioId = c.ComentarioId,
                    UsuarioId = c.UsuarioId,
                    ProductoId = c.ProductoId,
                    Comentario = c.Comentario,
                    Calificacion = c.Calificacion,
                    Fecha = c.Fecha
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ComentariosProductoDto>> GetComentario(int id)
        {
            var c = await _context.ComentariosProductos.FindAsync(id);
            if (c == null) return NotFound();

            return new ComentariosProductoDto
            {
                ComentarioId = c.ComentarioId,
                UsuarioId = c.UsuarioId,
                ProductoId = c.ProductoId,
                Comentario = c.Comentario,
                Calificacion = c.Calificacion,
                Fecha = c.Fecha
            };
        }

        [HttpPost]
        public async Task<ActionResult<ComentariosProductoDto>> PostComentario(ComentariosProductoDto dto)
        {
            var comentario = new ComentariosProducto
            {
                UsuarioId = dto.UsuarioId,
                ProductoId = dto.ProductoId,
                Comentario = dto.Comentario,
                Calificacion = dto.Calificacion,
                Fecha = dto.Fecha ?? DateTime.Now
            };

            _context.ComentariosProductos.Add(comentario);
            await _context.SaveChangesAsync();

            dto.ComentarioId = comentario.ComentarioId;
            return CreatedAtAction(nameof(GetComentario), new { id = comentario.ComentarioId }, dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComentario(int id)
        {
            var comentario = await _context.ComentariosProductos.FindAsync(id);
            if (comentario == null) return NotFound();

            _context.ComentariosProductos.Remove(comentario);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
