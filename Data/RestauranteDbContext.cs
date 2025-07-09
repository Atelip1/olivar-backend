using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using OlivarBackend.Models;

namespace OlivarBackend.Data;

public partial class RestauranteDbContext : DbContext
{
    public RestauranteDbContext()
    {
    }

    public RestauranteDbContext(DbContextOptions<RestauranteDbContext> options)
        : base(options)
    {
    }
    public DbSet<Egreso> Egresos { get; set; }
    public DbSet<DetallePedido> DetallesPedidos { get; set; }

    public virtual DbSet<Banner> Banners { get; set; }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<ComentariosProducto> ComentariosProductos { get; set; }

    public virtual DbSet<Configuracion> Configuracions { get; set; }

    public virtual DbSet<Consulta> Consultas { get; set; }

    public virtual DbSet<Cupone> Cupones { get; set; }

    public virtual DbSet<DetallePedido> DetallePedidos { get; set; }

    public virtual DbSet<DireccionesEntrega> DireccionesEntregas { get; set; }

    public virtual DbSet<Factura> Facturas { get; set; }

    public virtual DbSet<HistorialEstadoPedido> HistorialEstadoPedidos { get; set; }

    public virtual DbSet<ImagenesProducto> ImagenesProductos { get; set; }

    public virtual DbSet<LogsSistema> LogsSistemas { get; set; }

    public virtual DbSet<Notificacione> Notificaciones { get; set; }

    public virtual DbSet<Pago> Pagos { get; set; }

    public virtual DbSet<Pedido> Pedidos { get; set; }

    public virtual DbSet<Permiso> Permisos { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    public virtual DbSet<Reserva> Reservas { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Sucursale> Sucursales { get; set; }

    public virtual DbSet<Suscriptore> Suscriptores { get; set; }
    public virtual DbSet<RolPermiso> RolPermisos { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Banner>(entity =>
        {
            entity.HasKey(e => e.BannerId).HasName("PK__Banners__32E86AD142C2F01D");

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.ImagenUrl).HasMaxLength(255);
            entity.Property(e => e.Titulo).HasMaxLength(100);
        });

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.CategoriaId).HasName("PK__Categori__F353C1E57F62D38E");

            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<ComentariosProducto>(entity =>
        {
            entity.HasKey(e => e.ComentarioId).HasName("PK__Comentar__F1844938BC5623BD");

            entity.Property(e => e.Comentario).HasMaxLength(255);
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Producto).WithMany(p => p.ComentariosProductos)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comentari__Produ__76969D2E");

            entity.HasOne(d => d.Usuario).WithMany(p => p.ComentariosProductos)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Comentari__Usuar__75A278F5");
        });

        modelBuilder.Entity<Configuracion>(entity =>
        {
            entity.HasKey(e => e.Clave).HasName("PK__Configur__E8181E10ED89AB4F");

            entity.ToTable("Configuracion");

            entity.Property(e => e.Clave).HasMaxLength(50);
            entity.Property(e => e.Valor).HasMaxLength(255);
        });

        modelBuilder.Entity<Consulta>(entity =>
        {
            entity.HasKey(e => e.ConsultaId).HasName("PK__Consulta__7D0B7DCC153EF9C3");

            entity.Property(e => e.Asunto).HasMaxLength(100);
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .HasDefaultValue("Pendiente");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Mensaje).HasMaxLength(1000);

            entity.HasOne(d => d.Usuario).WithMany(p => p.Consulta)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Consultas__Usuar__00200768");
        });

        modelBuilder.Entity<Cupone>(entity =>
        {
            entity.HasKey(e => e.CuponId).HasName("PK__Cupones__C4356897D86D66B5");

            entity.HasIndex(e => e.Codigo, "UQ__Cupones__06370DAC9A3337F4").IsUnique();

            entity.Property(e => e.Activo).HasDefaultValue(true);
            entity.Property(e => e.Codigo).HasMaxLength(20);
        });

        modelBuilder.Entity<DetallePedido>(entity =>
        {
            entity.HasKey(e => e.DetallePedidoId).HasName("PK__DetalleP__6ED21C21B91B4040");

            entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Subtotal).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Pedido).WithMany(p => p.DetallePedidos)
                .HasForeignKey(d => d.PedidoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DetallePe__Pedid__68487DD7");

            entity.HasOne(d => d.Producto).WithMany(p => p.DetallePedidos)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DetallePe__Produ__693CA210");
        });

        modelBuilder.Entity<DireccionesEntrega>(entity =>
        {
            entity.HasKey(e => e.DireccionId).HasName("PK__Direccio__68906D645C8AE0B1");

            entity.ToTable("DireccionesEntrega");

            entity.Property(e => e.Activa).HasDefaultValue(true);
            entity.Property(e => e.Alias).HasMaxLength(50);
            entity.Property(e => e.Direccion).HasMaxLength(255);
            entity.Property(e => e.Distrito).HasMaxLength(100);
            entity.Property(e => e.Referencia).HasMaxLength(255);

            entity.HasOne(d => d.Usuario).WithMany(p => p.DireccionesEntregas)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Direccion__Usuar__52593CB8");
        });

        modelBuilder.Entity<Factura>(entity =>
        {
            entity.HasKey(e => e.FacturaId).HasName("PK__Facturas__5C024865B887FD28");

            entity.Property(e => e.DireccionFiscal).HasMaxLength(255);
            entity.Property(e => e.FechaEmision)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MontoTotal).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.RazonSocial).HasMaxLength(150);
            entity.Property(e => e.Ruc)
                .HasMaxLength(20)
                .HasColumnName("RUC");

            entity.HasOne(d => d.Pedido).WithMany(p => p.Facturas)
                .HasForeignKey(d => d.PedidoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Facturas__Pedido__03F0984C");
        });

        modelBuilder.Entity<HistorialEstadoPedido>(entity =>
        {
            entity.HasKey(e => e.EstadoId).HasName("PK__Historia__FEF86B00D17E9BA1");

            entity.ToTable("HistorialEstadoPedido");

            entity.Property(e => e.Estado).HasMaxLength(50);
            entity.Property(e => e.FechaHora)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Pedido).WithMany(p => p.HistorialEstadoPedidos)
                .HasForeignKey(d => d.PedidoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Historial__Pedid__6D0D32F4");
        });

        modelBuilder.Entity<ImagenesProducto>(entity =>
        {
            entity.HasKey(e => e.ImagenId).HasName("PK__Imagenes__0C7D20B780CC0D26");

            entity.ToTable("ImagenesProducto");

            entity.Property(e => e.EsPrincipal).HasDefaultValue(false);
            entity.Property(e => e.Url).HasMaxLength(255);

            entity.HasOne(d => d.Producto).WithMany(p => p.ImagenesProductos)
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ImagenesP__Produ__4BAC3F29");
        });

        modelBuilder.Entity<LogsSistema>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PK__LogsSist__5E548648C784FC70");

            entity.ToTable("LogsSistema");

            entity.Property(e => e.Accion).HasMaxLength(255);
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.TablaAfectada).HasMaxLength(50);

            entity.HasOne(d => d.Usuario).WithMany(p => p.LogsSistemas)
                .HasForeignKey(d => d.UsuarioId)
                .HasConstraintName("FK__LogsSiste__Usuar__07C12930");
        });
        modelBuilder.Entity<Notificacione>(entity =>
        {
            entity.HasKey(e => e.NotificacionId).HasName("PK__Notifica__BCC120243920B53E");

            entity.Property(e => e.FechaEnvio)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.Property(e => e.Leida).HasDefaultValue(false);
            entity.Property(e => e.Mensaje).HasMaxLength(255);
            entity.Property(e => e.Titulo).HasMaxLength(100);

            // 🔧 Ajuste importante aquí
            entity.HasOne(d => d.Usuario)
                .WithMany(p => p.Notificaciones)
                .HasForeignKey(d => d.UsuarioId)
                .IsRequired(false) // ✅ Permite que UsuarioId sea null
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificac__Usuar__7B5B524B");
        });



        modelBuilder.Entity<Pago>(entity =>
        {
            entity.HasKey(e => e.PagoId).HasName("PK__Pagos__F00B61381DD95361");

            entity.Property(e => e.FechaPago)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Metodo).HasMaxLength(50);
            entity.Property(e => e.Monto).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Pedido).WithMany(p => p.Pagos)
                .HasForeignKey(d => d.PedidoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Pagos__PedidoId__70DDC3D8");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(e => e.PedidoId).HasName("PK__Pedidos__09BA1430CD026C3C");

            entity.Property(e => e.DescuentoAplicado)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .HasDefaultValue("Pendiente");
            entity.Property(e => e.Fecha)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.MetodoEntrega)
                .HasMaxLength(50)
                .HasDefaultValue("Delivery");
            entity.Property(e => e.MetodoPago).HasMaxLength(50);
            entity.Property(e => e.Total).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Cupon).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.CuponId)
                .HasConstraintName("FK__Pedidos__CuponId__6477ECF3");

            entity.HasOne(d => d.Sucursal).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.SucursalId)
                .HasConstraintName("FK__Pedidos__Sucursa__656C112C");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Pedidos)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Pedidos__Usuario__6383C8BA");
        });

        modelBuilder.Entity<Permiso>(entity =>
        {
            entity.HasKey(e => e.PermisoId).HasName("PK__Permisos__96E0C723C3DFD14A");

            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.ProductoId).HasName("PK__Producto__A430AEA3C28F4034");

            entity.Property(e => e.Descripcion).HasMaxLength(255);
            entity.Property(e => e.Estado).HasDefaultValue(true);
            entity.Property(e => e.ImagenUrl).HasMaxLength(255);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Precio).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Categoria).WithMany(p => p.Productos)
                .HasForeignKey(d => d.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict)

                .HasConstraintName("FK__Productos__Categ__47DBAE45");
        });

        modelBuilder.Entity<Reserva>(entity =>
        {
            entity.HasKey(e => e.ReservaId).HasName("PK__Reservas__C3993763E41E628E");

            entity.Property(e => e.Estado)
                .HasMaxLength(50)
                .HasDefaultValue("Pendiente");
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Observaciones).HasMaxLength(255);

            entity.HasOne(d => d.Sucursal).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.SucursalId)
                .HasConstraintName("FK__Reservas__Sucurs__5812160E");

            entity.HasOne(d => d.Usuario).WithMany(p => p.Reservas)
                .HasForeignKey(d => d.UsuarioId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Reservas__Usuari__571DF1D5");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RolId).HasName("PK__Roles__F92302F1596FD51F");

            entity.HasIndex(e => e.Nombre, "UQ__Roles__75E3EFCF7F496BC6").IsUnique();

            entity.Property(e => e.Nombre).HasMaxLength(50);

        });

        modelBuilder.Entity<Sucursale>(entity =>
        {
            entity.HasKey(e => e.SucursalId).HasName("PK__Sucursal__6CB482E1DF4939D8");

            entity.Property(e => e.Activa).HasDefaultValue(true);
            entity.Property(e => e.Direccion).HasMaxLength(255);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Telefono).HasMaxLength(50);
        });

        modelBuilder.Entity<Suscriptore>(entity =>
        {
            entity.HasKey(e => e.SuscriptorId).HasName("PK__Suscript__03ED70B24674BBFE");

            entity.HasIndex(e => e.Email, "UQ__Suscript__A9D10534A5E7D865").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(e => e.UsuarioId).HasName("PK__Usuarios__2B3DE7B8C5D631BF");

            entity.HasIndex(e => e.Email, "UQ__Usuarios__A9D1053482EAC8B3").IsUnique();

            entity.Property(e => e.Apellido).HasMaxLength(100);
            entity.Property(e => e.Contrasena).HasMaxLength(255);
            entity.Property(e => e.Direccion).HasMaxLength(255);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FechaRegistro)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Telefono).HasMaxLength(20);

            entity.HasOne(d => d.Rol).WithMany(p => p.Usuarios)
                .HasForeignKey(d => d.RolId)
                .HasConstraintName("FK__Usuarios__RolId__4222D4EF");
        });

        modelBuilder.Entity<RolPermiso>(entity =>
            {
                entity.ToTable("RolPermiso");
                entity.HasKey(e => new { e.RolId, e.PermisoId });

                entity.HasOne(e => e.Rol)
                      .WithMany(r => r.RolPermisos)
                      .HasForeignKey(e => e.RolId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK_RolPermiso_Rol");

                entity.HasOne(e => e.Permiso)
                      .WithMany(p => p.RolPermisos)
                      .HasForeignKey(e => e.PermisoId)
                      .OnDelete(DeleteBehavior.ClientSetNull)
                      .HasConstraintName("FK_RolPermiso_Permiso");

            });

        OnModelCreatingPartial(modelBuilder);
        }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
