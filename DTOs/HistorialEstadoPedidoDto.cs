namespace OlivarBackend.DTOs
{
    public class HistorialEstadoPedidoDto
    {
        public int EstadoId { get; set; }
        public int PedidoId { get; set; }
        public string? Estado { get; set; }
        public DateTime? FechaHora { get; set; }
    }
}
