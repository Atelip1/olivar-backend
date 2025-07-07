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
                var smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("mariadelpilartasaycolaque@gmail.com", "lntv ywtr dwhi yqsi"),
                    EnableSsl = true
                };

                string cuerpo = $"<h2>Gracias por tu compra</h2>" +
                                $"<p>Total: S/. {dto.Resumen.Total}</p>" +
                                $"<p>Método de Pago: {dto.Resumen.MetodoPago}</p>" +
                                $"<p>Método de Entrega: {dto.Resumen.MetodoEntrega}</p>" +
                                $"<p>Fecha: {dto.Resumen.Fecha}</p>";

                var mail = new MailMessage("mariadelpilartasaycolaque@gmail.com", dto.Email)
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
}
