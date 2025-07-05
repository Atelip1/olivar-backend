using Microsoft.EntityFrameworkCore;
using OlivarBackend.Data;
using OlivarBackend.DTOs;

namespace OlivarBackend.Services
{
    public class LoginService
    {
        private readonly RestauranteDbContext _context;

        public LoginService(RestauranteDbContext context)
        {
            _context = context;
        }

        public async Task<UsuarioDto?> ValidarLoginAsync(LoginDto login)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Email == login.Email && u.Contrasena == login.Contrasena);

            if (usuario == null)
                return null;

            return new UsuarioDto
            {
                UsuarioId = usuario.UsuarioId,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                Telefono = usuario.Telefono,
                Direccion = usuario.Direccion,
                FechaRegistro = usuario.FechaRegistro,
                RolId = usuario.RolId,
                RolNombre = usuario.Rol?.Nombre
            };
        }
    }
}

