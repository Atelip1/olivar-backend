using Microsoft.AspNetCore.Mvc;
using OlivarBackend.DTOs;
using OlivarBackend.Services;

namespace OlivarBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly LoginService _loginService;

        public LoginController(LoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto login)
        {
            var usuario = await _loginService.ValidarLoginAsync(login);

            if (usuario == null)
                return Unauthorized(new { mensaje = "Correo o contraseña incorrectos." });

            return Ok(usuario);
        }
    }
}
