﻿namespace OlivarBackend.DTOs
{
    public class ProductoDto
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public string? ImagenUrl { get; set; }
        public int CategoriaId { get; set; }
        public string? CategoriaNombre { get; set; }
        public bool? Estado { get; set; }
    }
}
