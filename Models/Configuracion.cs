using System;
using System.Collections.Generic;

namespace OlivarBackend.Models;

public partial class Configuracion
{
    public string Clave { get; set; } = null!;

    public string? Valor { get; set; }
}
