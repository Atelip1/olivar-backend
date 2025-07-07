namespace OlivarBackend.Dto
{
    public class EmailFacturaDto
    {
        public required string? Email { get; set; }
        public required ResumenFactura Resumen { get; set; }
    }

    public class ResumenFactura
    {
        public decimal Total { get; set; }
        public string MetodoPago { get; set; }
        public string MetodoEntrega { get; set; }
        public string Fecha { get; set; }
    }
}
