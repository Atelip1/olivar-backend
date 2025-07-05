// DTO para mostrar consulta
public class ConsultaDetailDto
{
    public int ConsultaId { get; set; }
    public int UsuarioId { get; set; }
    public string? Asunto { get; set; }
    public string? Mensaje { get; set; }
    public string? Estado { get; set; }
    public DateTime? Fecha { get; set; }
}