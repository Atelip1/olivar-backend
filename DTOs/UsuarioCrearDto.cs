public class UsuarioCrearDto
{
    public string Nombre { get; set; } = null!;
    public string? Apellido { get; set; }
    public string Email { get; set; } = null!;
    public string Contrasena { get; set; } = null!;
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
    public int RolId { get; set; } = 2;
}
