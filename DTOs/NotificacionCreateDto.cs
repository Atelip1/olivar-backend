namespace OlivarBackend.Dtos
{
    public class NotificacionCreateDto
    {
        public int UsuarioId { get; set; }
        public string? Titulo { get; set; }
        public string? Mensaje { get; set; }
    }
}
