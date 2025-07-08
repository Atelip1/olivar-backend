using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OlivarBackend.Data;
using OlivarBackend.DTOs;
using OlivarBackend.Models;
using OlivarBackend.Services;
using System.Net.Mail;
using System.Net;

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
        // ✅ Recuperación de contraseña
        [AllowAnonymous]
        [HttpPost("enviar-recuperacion")]
        public async Task<IActionResult> EnviarCorreoRecuperacion([FromBody] RecuperacionDto dto)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (usuario == null)
                return NotFound(new { mensaje = "Correo no registrado." });

            var token = Guid.NewGuid().ToString();
            usuario.TokenRecuperacion = token;
            usuario.TokenExpiracion = DateTime.UtcNow.AddMinutes(15);
            await _context.SaveChangesAsync();

            string enlace = $"https://olivar-front.onrender.com/reset-password?token={token}";
            string asunto = "Recuperación de contraseña - Olivar";
            string cuerpo = $@"
        <h3>Hola {usuario.Nombre},</h3>
        <p>Recibimos una solicitud para restablecer tu contraseña.</p>
        <p>Haz clic en el siguiente enlace para continuar:</p>
        <p><a href='{enlace}' target='_blank'>Restablecer contraseña</a></p>
        <p><small>Este enlace expirará en 15 minutos.</small></p>";

            await EnviarCorreo(dto.Email, asunto, cuerpo);

            return Ok(new { mensaje = "Instrucciones enviadas al correo." });
        }

        // Método privado para enviar correo
        private async Task EnviarCorreo(string destino, string asunto, string cuerpoHtml)
        {
           
            var remitente = "mariadelpilartasaycolaque@gmail.com";         // <-- Usa el mismo correo que ya te funciona
            var clave = "xjdp evhf oxjp veij";         // <-- Usa la clave válida del correo

            var smtp = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(remitente, clave),
                EnableSsl = true
            };

            var mensaje = new MailMessage(remitente, destino, asunto, cuerpoHtml);
            mensaje.IsBodyHtml = true;

            await smtp.SendMailAsync(mensaje);
        }

        public class RestablecerPasswordDto
        {
            public string Token { get; set; }
            public string NuevaContrasena { get; set; }
        }

        [AllowAnonymous]
        [HttpPost("restablecer-password")]
        public async Task<IActionResult> RestablecerPassword([FromBody] RestablecerPasswordDto dto)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.TokenRecuperacion == dto.Token);
            if (usuario == null)
                return BadRequest(new { mensaje = "Token inválido." });

            if (usuario.TokenExpiracion == null || usuario.TokenExpiracion < DateTime.UtcNow)
                return BadRequest(new { mensaje = "El token expiró." });

            // Aquí deberías hashear la nueva contraseña antes de guardarla
            usuario.Contrasena = HashPassword(dto.NuevaContrasena);

            // Limpiar el token para que no se pueda reutilizar
            usuario.TokenRecuperacion = null;
            usuario.TokenExpiracion = null;

            await _context.SaveChangesAsync();

            return Ok(new { mensaje = "Contraseña actualizada correctamente." });
        }

        // Ejemplo simple de método para hashear contraseña, puedes usar cualquier método seguro (como BCrypt)
        private string HashPassword(string password)
        {
            // Esto es solo un ejemplo simple, no usar en producción sin un buen algoritmo de hash
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var bytes = System.Text.Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
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
