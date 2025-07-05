using System;
using System.Collections.Generic;

namespace OlivarBackend.Models;

public partial class Suscriptore
{
    public int SuscriptorId { get; set; }

    public string? Email { get; set; }

    public DateTime? FechaRegistro { get; set; }
}
