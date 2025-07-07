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
            var document = new PdfDocument();
            var page = document.AddPage();
            var gfx = XGraphics.FromPdfPage(page);

            // Usa directamente 1 para Bold si XFontStyle.Bold no existe
            var fontTitulo = new XFont("Helvetica", 18); // Regular
                                                       // 1 = Bold
            var fontTexto = new XFont("Helvetica", 12);


            double y = 40;

            gfx.DrawString("Factura de Compra", fontTitulo, XBrushes.Black,
                new XRect(0, y, page.Width, 30), XStringFormats.TopCenter);

            y += 50;
            gfx.DrawString($"Correo: {dto.Email}", fontTexto, XBrushes.Black,
                new XRect(40, y, page.Width, 20), XStringFormats.TopLeft);
            y += 25;
            gfx.DrawString($"Fecha: {dto.Resumen.Fecha}", fontTexto, XBrushes.Black,
                new XRect(40, y, page.Width, 20), XStringFormats.TopLeft);
            y += 25;
            gfx.DrawString($"Método de Pago: {dto.Resumen.MetodoPago}", fontTexto, XBrushes.Black,
                new XRect(40, y, page.Width, 20), XStringFormats.TopLeft);
            y += 25;
            gfx.DrawString($"Método de Entrega: {dto.Resumen.MetodoEntrega}", fontTexto, XBrushes.Black,
                new XRect(40, y, page.Width, 20), XStringFormats.TopLeft);
            y += 25;
            gfx.DrawString($"Total: S/. {dto.Resumen.Total:F2}", fontTexto, XBrushes.Black,
                new XRect(40, y, page.Width, 20), XStringFormats.TopLeft);

            using (var stream = new MemoryStream())
            {
                document.Save(stream, false);
                return stream.ToArray();
            }
        }
    }
}
