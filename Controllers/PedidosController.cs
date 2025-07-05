using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OlivarBackend.Data;
using OlivarBackend.DTOs;
using OlivarBackend.Models;

namespace OlivarBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly RestauranteDbContext _context;

        public PedidosController(RestauranteDbContext context)
        {
            _context = context;
        }

        // ✅ GET: /api/Pedidos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PedidoDto>>> GetPedidos()
        {
            var pedidos = await _context.Pedidos
                .Include(p => p.Usuario)
                .Select(p => new PedidoDto
                {
                    PedidoId = p.PedidoId,
                    UsuarioId = p.UsuarioId,
                    Fecha = p.Fecha,
                    Estado = p.Estado,
                    MetodoPago = p.MetodoPago,
                    MetodoEntrega = p.MetodoEntrega,
                    CuponId = p.CuponId,
                    DescuentoAplicado = p.DescuentoAplicado,
                    Total = p.Total,
                    SucursalId = p.SucursalId,
                    NombreUsuario = p.Usuario.Nombre
                })
                .ToListAsync();

            return Ok(pedidos);
        }

        // ✅ GET: /api/Pedidos/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<PedidoDto>> GetPedido(int id)
        {
            var pedido = await _context.Pedidos
                .Include(p => p.Usuario)
                .Where(p => p.PedidoId == id)
                .Select(p => new PedidoDto
                {
                    PedidoId = p.PedidoId,
                    UsuarioId = p.UsuarioId,
                    Fecha = p.Fecha,
                    Estado = p.Estado,
                    MetodoPago = p.MetodoPago,
                    MetodoEntrega = p.MetodoEntrega,
                    CuponId = p.CuponId,
                    DescuentoAplicado = p.DescuentoAplicado,
                    Total = p.Total,
                    SucursalId = p.SucursalId,
                    NombreUsuario = p.Usuario.Nombre
                })
                .FirstOrDefaultAsync();

            if (pedido == null)
                return NotFound();

            return Ok(pedido);
        }

        // ✅ POST: /api/Pedidos
        [HttpPost]
        public async Task<ActionResult<PedidoDto>> PostPedido(PedidoDto dto)
        {
            var pedido = new Pedido
            {
                UsuarioId = dto.UsuarioId,
                Fecha = dto.Fecha ?? DateTime.Now,
                Estado = dto.Estado ?? "Pendiente",
                MetodoPago = dto.MetodoPago,
                MetodoEntrega = dto.MetodoEntrega,
                CuponId = dto.CuponId,
                DescuentoAplicado = dto.DescuentoAplicado,
                Total = dto.Total,
                SucursalId = dto.SucursalId
            };

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            dto.PedidoId = pedido.PedidoId;
            dto.Fecha = pedido.Fecha;

            return CreatedAtAction(nameof(GetPedido), new { id = pedido.PedidoId }, dto);
        }

        // ✅ PUT: /api/Pedidos/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPedido(int id, PedidoDto dto)
        {
            if (id != dto.PedidoId)
                return BadRequest();

            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
                return NotFound();

            pedido.UsuarioId = dto.UsuarioId;
            pedido.Fecha = dto.Fecha ?? pedido.Fecha;
            pedido.Estado = dto.Estado;
            pedido.MetodoPago = dto.MetodoPago;
            pedido.MetodoEntrega = dto.MetodoEntrega;
            pedido.CuponId = dto.CuponId;
            pedido.DescuentoAplicado = dto.DescuentoAplicado;
            pedido.Total = dto.Total;
            pedido.SucursalId = dto.SucursalId;

            _context.Entry(pedido).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ✅ DELETE: /api/Pedidos/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePedido(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido == null)
                return NotFound();

            _context.Pedidos.Remove(pedido);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
