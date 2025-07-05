using System;
using System.Collections.Generic;

namespace OlivarBackend.Models;

public partial class Producto
{
    public int ProductoId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal Precio { get; set; }

    public string? ImagenUrl { get; set; }

    public int CategoriaId { get; set; }

    public bool? Estado { get; set; }

    public virtual Categoria Categoria { get; set; } = null!;

    public virtual ICollection<ComentariosProducto> ComentariosProductos { get; set; } = new List<ComentariosProducto>();

    public virtual ICollection<DetallePedido> DetallePedidos { get; set; } = new List<DetallePedido>();

    public virtual ICollection<ImagenesProducto> ImagenesProductos { get; set; } = new List<ImagenesProducto>();
}
