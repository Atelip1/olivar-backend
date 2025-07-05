using System;
using System.Collections.Generic;

namespace OlivarBackend.Models;

public partial class Consulta
{
    public int ConsultaId { get; set; }

    public int UsuarioId { get; set; }

    public string? Asunto { get; set; }

    public string? Mensaje { get; set; }

    public string? Estado { get; set; }

    public DateTime? Fecha { get; set; }

    public virtual Usuario Usuario { get; set; } = null!;
}
