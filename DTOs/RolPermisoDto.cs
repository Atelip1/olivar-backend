namespace OlivarBackend.DTOs
{
    public class RolPermisoDTO
    {
        public int RolId { get; set; }
        public string RolNombre { get; set; } = string.Empty;

        public int PermisoId { get; set; }
        public string PermisoNombre { get; set; } = string.Empty;
    }
}
