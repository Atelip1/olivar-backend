namespace OlivarBackend.Dto
{
    public class EmailFacturaDto
    {
        public string Email { get; set; } = string.Empty;

        public ResumenFacturaDto Resumen { get; set; } = new();
    }


    public class ResumenFacturaDto
    {
        public decimal Total { get; set; }
        public string MetodoPago { get; set; } = string.Empty;
        public string MetodoEntrega { get; set; } = string.Empty;
        public string Fecha { get; set; } = string.Empty;
    }

}
