using Microsoft.EntityFrameworkCore;
using OlivarBackend.Data;
using OlivarBackend.DTOs;
using OlivarBackend.Models;

namespace OlivarBackend.Services
{
    public class PermisoService : IPermisoService
    {
        private readonly RestauranteDbContext _context;

        public PermisoService(RestauranteDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PermisoDTO>> GetAllAsync()
        {
            return await _context.Permisos
                .Select(p => new PermisoDTO
                {
                    PermisoId = p.PermisoId,
                    Nombre = p.Nombre
                }).ToListAsync();
        }

        public async Task<PermisoDTO?> GetByIdAsync(int id)
        {
            var permiso = await _context.Permisos.FindAsync(id);
            if (permiso == null) return null;

            return new PermisoDTO { PermisoId = permiso.PermisoId, Nombre = permiso.Nombre };
        }

        public async Task<PermisoDTO> CreateAsync(PermisoDTO dto)
        {
            var permiso = new Permiso { Nombre = dto.Nombre };
            _context.Permisos.Add(permiso);
            await _context.SaveChangesAsync();

            dto.PermisoId = permiso.PermisoId;
            return dto;
        }

        public async Task<bool> UpdateAsync(int id, PermisoDTO dto)
        {
            var permiso = await _context.Permisos.FindAsync(id);
            if (permiso == null) return false;

            permiso.Nombre = dto.Nombre;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var permiso = await _context.Permisos.FindAsync(id);
            if (permiso == null) return false;

            _context.Permisos.Remove(permiso);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
