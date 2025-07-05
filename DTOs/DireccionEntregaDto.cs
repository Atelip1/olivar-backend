namespace OlivarBackend.DTOs
{
    public class DireccionEntregaDto
    {
        public int? DireccionId { get; set; }
        public int UsuarioId { get; set; }
        public string? Alias { get; set; }
        public string? Direccion { get; set; }
        public string? Referencia { get; set; }
        public string? Distrito { get; set; }
        public double? Latitud { get; set; }
        public double? Longitud { get; set; }
        public bool? Activa { get; set; }
    }
}
