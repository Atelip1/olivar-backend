using System;
using System.Collections.Generic;

namespace OlivarBackend.Models;

public partial class DireccionesEntrega
{
    public int DireccionId { get; set; }

    public int UsuarioId { get; set; }

    public string? Alias { get; set; }

    public string? Direccion { get; set; }

    public string? Referencia { get; set; }

    public string? Distrito { get; set; }

    public double? Latitud { get; set; }

    public double? Longitud { get; set; }

    public bool? Activa { get; set; }

    public virtual Usuario Usuario { get; set; } = null!;
}
