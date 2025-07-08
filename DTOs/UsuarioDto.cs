namespace OlivarBackend.DTOs
{
    public class UsuarioDto
    {
        public int UsuarioId { get; set; }
        public string Nombre { get; set; } = null!;
        public string? Apellido { get; set; }
        public string Email { get; set; } = null!;
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public int? RolId { get; set; }
        public string? RolNombre { get; set; }

        public string? TokenRecuperacion { get; set; }
        public DateTime? TokenExpiracion { get; set; }

    }
}
