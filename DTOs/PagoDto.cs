namespace OlivarBackend.DTOs
{
    public class PagoDto
    {
        public int PagoId { get; set; }
        public int PedidoId { get; set; }
        public decimal? Monto { get; set; }
        public string? Metodo { get; set; }
        public DateTime? FechaPago { get; set; }
    }
}
