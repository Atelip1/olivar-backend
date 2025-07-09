using Microsoft.AspNetCore.Mvc;
using OlivarBackend.Dto;
using OlivarBackend.Services;
using System.IO;
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
                var pdfBytes = GeneradorFacturaPdf.GenerarFacturaPdf(dto);

                var smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    Credentials = new NetworkCredential("mariadelpilartasaycolaque@gmail.com", "xjdp evhf oxjp veij"),
                    EnableSsl = true
                };

                var mail = new MailMessage("mariadelpilartasaycolaque@gmail.com", dto.Email)
                {
                    Subject = "🧾 Factura PDF de tu pedido",
                    Body = "Adjunto encontrarás tu factura en formato PDF.\n\nGracias por tu compra.",
                    IsBodyHtml = false
                };

                mail.Attachments.Add(new Attachment(new MemoryStream(pdfBytes), "FacturaPedido.pdf", "application/pdf"));

                smtp.Send(mail);
                return Ok(new { mensaje = "Factura enviada correctamente en PDF." });
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al enviar el correo", detalle = ex.Message });
            }
        }
    }
}
