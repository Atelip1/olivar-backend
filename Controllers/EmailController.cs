using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mail;

namespace OlivarBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        [HttpPost("enviar-factura")]
        public IActionResult EnviarFactura([FromBody] EmailFacturaDto dto)
        {
            try
            {
                // Configura tu servidor SMTP (esto es un ejemplo usando Gmail)
                var smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("tucorreo@gmail.com", "tu_contraseña_o_app_password"),
                    EnableSsl = true
                };

                string cuerpo = $"<h2>Gracias por tu compra</h2>" +
                                $"<p>Total: S/. {dto.resumen.total}</p>" +
                                $"<p>Método de Pago: {dto.resumen.metodoPago}</p>" +
                                $"<p>Método de Entrega: {dto.resumen.metodoEntrega}</p>" +
                                $"<p>Fecha: {dto.resumen.fecha}</p>";

                var mail = new MailMessage("tucorreo@gmail.com", dto.email)
                {
                    Subject = "🧾 Factura de tu pedido",
                    Body = cuerpo,
                    IsBodyHtml = true
                };

                smtp.Send(mail);
                return Ok(new { mensaje = "Factura enviada correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al enviar el correo", detalle = ex.Message });
            }
        }
    }

    public class EmailFacturaDto
    {
        public string email { get; set; }
        public dynamic resumen { get; set; }  // puedes usar un modelo más específico si deseas
    }
}
