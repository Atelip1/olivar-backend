using System;
using System.Collections.Generic;

namespace OlivarBackend.Models;

public partial class LogsSistema
{
    public int LogId { get; set; }

    public int? UsuarioId { get; set; }

    public string? Accion { get; set; }

    public string? TablaAfectada { get; set; }

    public DateTime? Fecha { get; set; }

    public virtual Usuario? Usuario { get; set; }
}
