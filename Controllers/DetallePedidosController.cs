using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OlivarBackend.Data;
using OlivarBackend.DTOs;
using OlivarBackend.Models;

namespace OlivarBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DetallePedidosController : ControllerBase
    {
        private readonly RestauranteDbContext _context;

        public DetallePedidosController(RestauranteDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetallePedidoDto>>> GetDetalles()
        {
            var detalles = await _context.DetallePedidos
                .Include(d => d.Producto)
                .Select(d => new DetallePedidoDto
                {
                    DetallePedidoId = d.DetallePedidoId,
                    PedidoId = d.PedidoId,
                    ProductoId = d.ProductoId,
                    NombreProducto = d.Producto.Nombre,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario
                }).ToListAsync();

            return Ok(detalles);
        }

        [HttpPost]
        public async Task<ActionResult<DetallePedidoDto>> PostDetalle(DetallePedidoDto dto)
        {
            var detalle = new DetallePedido
            {
                PedidoId = dto.PedidoId,
                ProductoId = dto.ProductoId,
                Cantidad = dto.Cantidad,
                PrecioUnitario = dto.PrecioUnitario
            };

            _context.DetallePedidos.Add(detalle);
            await _context.SaveChangesAsync();

            // Obtener nombre del producto
            var producto = await _context.Productos.FindAsync(dto.ProductoId);
            dto.DetallePedidoId = detalle.DetallePedidoId;
            dto.NombreProducto = producto?.Nombre;

            return CreatedAtAction(nameof(PostDetalle), new { id = detalle.DetallePedidoId }, dto);
        }

        [HttpGet("por-pedido/{pedidoId}")]
        public async Task<ActionResult<IEnumerable<DetallePedidoDto>>> GetPorPedido(int pedidoId)
        {
            var detalles = await _context.DetallesPedidos
                .Where(d => d.PedidoId == pedidoId)
                .Include(d => d.Producto)
                .Select(d => new DetallePedidoDto
                {
                    DetallePedidoId = d.DetallePedidoId,
                    PedidoId = d.PedidoId,
                    ProductoId = d.ProductoId,
                    NombreProducto = d.Producto.Nombre,
                    Cantidad = d.Cantidad,
                    PrecioUnitario = d.PrecioUnitario
                })
                .ToListAsync();

            if (detalles == null || detalles.Count == 0)
            {
                return NotFound("No se encontraron productos para este pedido.");
            }

            return Ok(detalles);
        }



    }
}
