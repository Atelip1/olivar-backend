using System;
using System.Collections.Generic;

namespace OlivarBackend.Models;

public partial class ImagenesProducto
{
    public int ImagenId { get; set; }

    public int ProductoId { get; set; }

    public string? Url { get; set; }

    public bool? EsPrincipal { get; set; }

    public virtual Producto Producto { get; set; } = null!;
}
