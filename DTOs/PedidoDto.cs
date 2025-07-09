namespace OlivarBackend.DTOs
{
    public class PedidoDto
    {
        public int PedidoId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime? Fecha { get; set; }
        public string? Estado { get; set; }
        public string? MetodoPago { get; set; }
        public string? MetodoEntrega { get; set; }
        public int? CuponId { get; set; }
        public decimal? DescuentoAplicado { get; set; }
        public decimal Total { get; set; }
        public int? SucursalId { get; set; }

        public string? NombreUsuario { get; set; } 
    }
}
