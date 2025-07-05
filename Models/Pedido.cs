using System;
using System.Collections.Generic;

namespace OlivarBackend.Models;

public partial class Pedido
{
    public int PedidoId { get; set; }

    public int UsuarioId { get; set; }

    public DateTime? Fecha { get; set; }

    public string? Estado { get; set; }

    public string? MetodoPago { get; set; }

    public string? MetodoEntrega { get; set; }

    public int? CuponId { get; set; }

    public decimal? DescuentoAplicado { get; set; }

    public decimal Total { get; set; }

    public int? SucursalId { get; set; }

    public virtual Cupone? Cupon { get; set; }

    public virtual ICollection<DetallePedido> DetallePedidos { get; set; } = new List<DetallePedido>();

    public virtual ICollection<Factura> Facturas { get; set; } = new List<Factura>();

    public virtual ICollection<HistorialEstadoPedido> HistorialEstadoPedidos { get; set; } = new List<HistorialEstadoPedido>();

    public virtual ICollection<Pago> Pagos { get; set; } = new List<Pago>();

    public virtual Sucursale? Sucursal { get; set; }

    public virtual Usuario Usuario { get; set; } = null!;
}
