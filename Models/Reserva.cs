using System;
using System.Collections.Generic;

namespace OlivarBackend.Models;

public partial class Reserva
{
    public int ReservaId { get; set; }

    public int UsuarioId { get; set; }

    public DateOnly Fecha { get; set; }

    public TimeOnly Hora { get; set; }

    public int CantidadPersonas { get; set; }

    public string? Observaciones { get; set; }

    public string? Estado { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public int? SucursalId { get; set; }

    public virtual Sucursale? Sucursal { get; set; }

    public virtual Usuario Usuario { get; set; } = null!;
}
