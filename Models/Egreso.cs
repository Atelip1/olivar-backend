namespace OlivarBackend.Models
{
    public class Egreso
    {
        public int EgresoId { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }
        public decimal Monto { get; set; }
    }
}
