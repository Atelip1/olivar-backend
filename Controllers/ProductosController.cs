using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OlivarBackend.Data;
using OlivarBackend.DTOs;
using OlivarBackend.Models;

namespace OlivarBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class ProductosController : ControllerBase
    {
        private readonly RestauranteDbContext _context;

        public ProductosController(RestauranteDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> GetProductos()
        {
            var productos = await _context.Productos
                .Include(p => p.Categoria)
                .Where(p => p.Estado == true) // Solo activos
                .Select(p => new ProductoDto
                {
                    ProductoId = p.ProductoId,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    Precio = p.Precio,
                    ImagenUrl = p.ImagenUrl,
                    CategoriaId = p.CategoriaId,
                    CategoriaNombre = p.Categoria.Nombre,
                    Estado = p.Estado
                })
                .ToListAsync();

            return Ok(productos);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoDto>> GetProducto(int id)
        {
            var producto = await _context.Productos
                .Include(p => p.Categoria)
                .Where(p => p.ProductoId == id)
                .Select(p => new ProductoDto
                {
                    ProductoId = p.ProductoId,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    Precio = p.Precio,
                    ImagenUrl = p.ImagenUrl,
                    CategoriaId = p.CategoriaId,
                    CategoriaNombre = p.Categoria.Nombre,
                    Estado = p.Estado
                })
                .FirstOrDefaultAsync();

            if (producto == null)
                return NotFound();

            return Ok(producto);
        }

        [HttpPost]
        public async Task<ActionResult<ProductoDto>> PostProducto(ProductoDto dto)
        {
            var producto = new Producto
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Precio = dto.Precio,
                ImagenUrl = dto.ImagenUrl,
                CategoriaId = dto.CategoriaId,
                Estado = dto.Estado ?? true
            };

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();

            var categoria = await _context.Categorias.FindAsync(producto.CategoriaId);
            dto.ProductoId = producto.ProductoId;
            dto.CategoriaNombre = categoria?.Nombre;

            return CreatedAtAction(nameof(GetProducto), new { id = producto.ProductoId }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProducto(int id, ProductoDto dto)
        {
            if (id != dto.ProductoId)
                return BadRequest();

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
                return NotFound();

            producto.Nombre = dto.Nombre;
            producto.Descripcion = dto.Descripcion;
            producto.Precio = dto.Precio;
            producto.ImagenUrl = dto.ImagenUrl;
            producto.CategoriaId = dto.CategoriaId;
            producto.Estado = dto.Estado;

            _context.Entry(producto).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
                return NotFound();

            producto.Estado = false; // Eliminación lógica
            _context.Entry(producto).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
