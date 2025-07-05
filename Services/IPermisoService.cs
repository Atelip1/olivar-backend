using OlivarBackend.DTOs;

namespace OlivarBackend.Services
{
    public interface IPermisoService
    {
        Task<IEnumerable<PermisoDTO>> GetAllAsync();
        Task<PermisoDTO?> GetByIdAsync(int id);
        Task<PermisoDTO> CreateAsync(PermisoDTO dto);
        Task<bool> UpdateAsync(int id, PermisoDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
