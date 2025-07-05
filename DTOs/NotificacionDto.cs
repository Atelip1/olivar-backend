namespace OlivarBackend.Dtos
{
    public class NotificacionDto
    {
        public int NotificacionId { get; set; }
        public int UsuarioId { get; set; }
        public string? Titulo { get; set; }
        public string? Mensaje { get; set; }
        public DateTime? FechaEnvio { get; set; }
        public bool? Leida { get; set; }
    }
}
