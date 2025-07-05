namespace OlivarBackend.DTOs
{
    public class FacturaDto
    {
        public int FacturaId { get; set; }
        public int PedidoId { get; set; }
        public string? RazonSocial { get; set; }
        public string? Ruc { get; set; }
        public string? DireccionFiscal { get; set; }
        public DateTime? FechaEmision { get; set; }
        public decimal? MontoTotal { get; set; }
    }
}
