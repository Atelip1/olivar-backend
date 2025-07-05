namespace OlivarBackend.DTOs
{
    public class ReservaDto
    {
        public int ReservaId { get; set; }
        public int UsuarioId { get; set; }
        public DateOnly Fecha { get; set; }
        public TimeOnly Hora { get; set; }
        public int CantidadPersonas { get; set; }
        public string? Observaciones { get; set; }
        public string? Estado { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public int? SucursalId { get; set; }

        public string? NombreUsuario { get; set; }
        public string? NombreSucursal { get; set; }
    }
}
