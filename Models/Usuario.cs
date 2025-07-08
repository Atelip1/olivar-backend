using System;
using System.Collections.Generic;

namespace OlivarBackend.Models;

public partial class Usuario
{
    public int UsuarioId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Apellido { get; set; }

    public string Email { get; set; } = null!;

    public string Contrasena { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? Direccion { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public int? RolId { get; set; }





    public virtual ICollection<ComentariosProducto> ComentariosProductos { get; set; } = new List<ComentariosProducto>();

    public virtual ICollection<Consulta> Consulta { get; set; } = new List<Consulta>();

    public virtual ICollection<DireccionesEntrega> DireccionesEntregas { get; set; } = new List<DireccionesEntrega>();

    public virtual ICollection<LogsSistema> LogsSistemas { get; set; } = new List<LogsSistema>();

    public virtual ICollection<Notificacione> Notificaciones { get; set; } = new List<Notificacione>();

    public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

    public virtual ICollection<Reserva> Reservas { get; set; } = new List<Reserva>();

    public virtual Role? Rol { get; set; }
}
