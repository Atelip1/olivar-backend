namespace OlivarBackend.DTOs
{
    public class CuponDto
    {
        public int CuponId { get; set; }
        public string? Codigo { get; set; }
        public int? DescuentoPorcentaje { get; set; }
        public DateOnly? FechaInicio { get; set; }
        public DateOnly? FechaFin { get; set; }
        public bool? Activo { get; set; }
    }
}
