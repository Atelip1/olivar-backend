using System;
using System.Collections.Generic;

namespace OlivarBackend.Models;

public partial class Pago
{
    public int PagoId { get; set; }

    public int PedidoId { get; set; }

    public decimal? Monto { get; set; }

    public string? Metodo { get; set; }

    public DateTime? FechaPago { get; set; }

    public virtual Pedido Pedido { get; set; } = null!;
}
