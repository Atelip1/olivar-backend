namespace OlivarBackend.DTOs
{
    public class ComentariosProductoDto
    {
        public int ComentarioId { get; set; }
        public int UsuarioId { get; set; }
        public int ProductoId { get; set; }
        public string? Comentario { get; set; }
        public int? Calificacion { get; set; }
        public DateTime? Fecha { get; set; }
    }
}
