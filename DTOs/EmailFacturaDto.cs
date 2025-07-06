public class EmailFacturaDto
{
    public string Email { get; set; } = string.Empty;
    public FacturaResumen Resumen { get; set; } = new FacturaResumen();
}

public class FacturaResumen
{
    public double Total { get; set; }
    public string MetodoPago { get; set; } = string.Empty;
    public string MetodoEntrega { get; set; } = string.Empty;
    public string Fecha { get; set; } = string.Empty;
}
