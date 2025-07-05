using System;
using System.Collections.Generic;

namespace OlivarBackend.Models;

public partial class ComentariosProducto
{
    public int ComentarioId { get; set; }

    public int UsuarioId { get; set; }

    public int ProductoId { get; set; }

    public string? Comentario { get; set; }

    public int? Calificacion { get; set; }

    public DateTime? Fecha { get; set; }

    public virtual Producto Producto { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
