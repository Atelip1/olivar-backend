using System;
using System.Collections.Generic;

namespace OlivarBackend.Models;

public partial class Cupone
{
    public int CuponId { get; set; }

    public string? Codigo { get; set; }

    public int? DescuentoPorcentaje { get; set; }

    public DateOnly? FechaInicio { get; set; }

    public DateOnly? FechaFin { get; set; }

    public bool? Activo { get; set; }

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
