using System;
using System.Collections.Generic;

namespace OlivarBackend.Models;

public partial class HistorialEstadoPedido
{
    public int EstadoId { get; set; }

    public int PedidoId { get; set; }

    public string? Estado { get; set; }

    public DateTime? FechaHora { get; set; }

    public virtual Pedido Pedido { get; set; } = null!;
}
