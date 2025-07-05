using System;
using System.Collections.Generic;

namespace OlivarBackend.Models;

public partial class Factura
{
    public int FacturaId { get; set; }

    public int PedidoId { get; set; }

    public string? RazonSocial { get; set; }

    public string? Ruc { get; set; }

    public string? DireccionFiscal { get; set; }

    public DateTime? FechaEmision { get; set; }

    public decimal? MontoTotal { get; set; }

    public virtual Pedido Pedido { get; set; } = null!;
}
