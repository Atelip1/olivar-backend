using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.IO;
using OlivarBackend.Dto;

namespace OlivarBackend.Services
{
    public class GeneradorFacturaPdf
    {
        public static byte[] GenerarFacturaPdf(EmailFacturaDto dto)
        {
            using (var documento = new PdfDocument())
            {
                var pagina = documento.AddPage();
                var gfx = XGraphics.FromPdfPage(pagina);

                var fontTitulo = new XFont("Verdana", 18, XFontStyleEx.Bold);
                var fontSubtitulo = new XFont("Verdana", 12, XFontStyleEx.Bold);
                var fontTexto = new XFont("Verdana", 12, XFontStyleEx.Bold);


                double y = 40;
                gfx.DrawString("Factura de Compra", fontTitulo, XBrushes.Black, new XRect(0, y, pagina.Width, 30), XStringFormats.TopCenter);

                y += 50;
                gfx.DrawString($"Correo: {dto.Email}", fontTexto, XBrushes.Black, new XRect(40, y, pagina.Width, 20), XStringFormats.TopLeft);
                y += 25;
                gfx.DrawString($"Fecha: {dto.Resumen.Fecha}", fontTexto, XBrushes.Black, new XRect(40, y, pagina.Width, 20), XStringFormats.TopLeft);
                y += 25;
                gfx.DrawString($"Método de Pago: {dto.Resumen.MetodoPago}", fontTexto, XBrushes.Black, new XRect(40, y, pagina.Width, 20), XStringFormats.TopLeft);
                y += 25;
                gfx.DrawString($"Método de Entrega: {dto.Resumen.MetodoEntrega}", fontTexto, XBrushes.Black, new XRect(40, y, pagina.Width, 20), XStringFormats.TopLeft);
                y += 25;
                gfx.DrawString($"Total: S/. {dto.Resumen.Total}", fontTexto, XBrushes.Black, new XRect(40, y, pagina.Width, 20), XStringFormats.TopLeft);

                using (var stream = new MemoryStream())
                {
                    documento.Save(stream, false);
                    return stream.ToArray();
                }
            }
        }
    }
}
