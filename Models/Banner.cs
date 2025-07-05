using System;
using System.Collections.Generic;

namespace OlivarBackend.Models;

public partial class Banner
{
    public int BannerId { get; set; }

    public string? Titulo { get; set; }

    public string? ImagenUrl { get; set; }

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public bool? Activo { get; set; }
}
