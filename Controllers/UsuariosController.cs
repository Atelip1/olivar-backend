using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OlivarBackend.Data;
using OlivarBackend.DTOs;
using OlivarBackend.Models;
using OlivarBackend.Services;

namespace OlivarBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly RestauranteDbContext _context;
        private readonly TokenService _tokenService;

        public UsuariosController(RestauranteDbContext context, TokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        // 🚫 Login
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (usuario == null || usuario.Contrasena != request.Contrasena)
                return Unauthorized(new { mensaje = "Usuario o contraseña incorrectos" });

            if (usuario.Rol == null || string.IsNullOrEmpty(usuario.Rol.Nombre))
                return StatusCode(500, new { mensaje = "El usuario no tiene un rol válido asignado." });

            var token = _tokenService.GenerateToken(
                usuario.UsuarioId.ToString(),
                usuario.Email,
                usuario.Rol.Nombre
            );

            return Ok(new
            {
                usuarioId = usuario.UsuarioId,
                email = usuario.Email,
                nombre = usuario.Nombre,        // 👈 Aquí lo agregamos
                rol = usuario.Rol.Nombre,
                token = token
            });
        }



        // ✅ Registro de usuario
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<UsuarioDto>> PostUsuario([FromBody] UsuarioCrearDto dto)
        {
            try
            {
                var existe = await _context.Usuarios.AnyAsync(u => u.Email == dto.Email);
                if (existe)
                    return Conflict(new { mensaje = "El correo ya está registrado." });

                var usuario = new Usuario
                {
                    Nombre = dto.Nombre,
                    Apellido = dto.Apellido,
                    Email = dto.Email,
                    Contrasena = dto.Contrasena,
                    Telefono = dto.Telefono,
                    Direccion = dto.Direccion,
                    FechaRegistro = DateTime.UtcNow,
                    RolId = dto.RolId
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                var rol = await _context.Roles.FindAsync(usuario.RolId);

                var usuarioDto = new UsuarioDto
                {
                    UsuarioId = usuario.UsuarioId,
                    Nombre = usuario.Nombre,
                    Apellido = usuario.Apellido,
                    Email = usuario.Email,
                    Telefono = usuario.Telefono,
                    Direccion = usuario.Direccion,
                    FechaRegistro = usuario.FechaRegistro,
                    RolId = usuario.RolId,
                    RolNombre = rol?.Nombre
                };

                return CreatedAtAction(nameof(GetUsuarioById), new { id = usuario.UsuarioId }, usuarioDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    mensaje = "❌ Error al registrar el usuario.",
                    exception = ex.Message,
                    inner = ex.InnerException?.Message,
                    detalle = ex.InnerException?.Message ?? ex.Message
                });
            }

        }


        // 🔒 Obtener todos
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetUsuarios()
        {
            var usuarios = await _context.Usuarios.Include(u => u.Rol).ToListAsync();

            var usuariosDto = usuarios.Select(u => new UsuarioDto
            {
                UsuarioId = u.UsuarioId,
                Nombre = u.Nombre,
                Apellido = u.Apellido,
                Email = u.Email,
                Telefono = u.Telefono,
                Direccion = u.Direccion,
                FechaRegistro = u.FechaRegistro,
                RolId = u.RolId,
                RolNombre = u.Rol?.Nombre
            });

            return Ok(usuariosDto);
        }

        // 🔒 Obtener por ID
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDto>> GetUsuarioById(int id)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.UsuarioId == id);

            if (usuario == null)
                return NotFound();

            var usuarioDto = new UsuarioDto
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

            return Ok(usuarioDto);
        }

        // 🔒 Editar
        [AllowAnonymous]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.UsuarioId)
                return BadRequest();

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // 🔒 Eliminar
        [AllowAnonymous]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound();

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(u => u.UsuarioId == id);
        }
    }
}
