using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.IO;
using OlivarBackend.Dto;
using PdfSharp.Fonts;
using OlivarBackend.Services;

namespace OlivarBackend.Services
{
    public class GeneradorFacturaPdf
    {
        public static byte[] GenerarFacturaPdf(EmailFacturaDto dto)
        {
            // Registrar solo una vez el resolutor de fuente
            GlobalFontSettings.FontResolver ??= new CustomFontResolver();

            var pdf = new PdfDocument();
            var page = pdf.AddPage();
            var gfx = XGraphics.FromPdfPage(page);

            var fontTitulo = new XFont("Verdana", 18, XFontStyleEx.Bold);
            var fontTexto = new XFont("Verdana", 12, XFontStyleEx.Regular);

            double y = 40;

            gfx.DrawString("Factura de Compra", fontTitulo, XBrushes.Black,
                new XRect(0, y, page.Width, 30), XStringFormats.TopCenter);

            y += 50;
            gfx.DrawString($"Correo: {dto.Email}", fontTexto, XBrushes.Black, new XRect(40, y, page.Width, 20), XStringFormats.TopLeft);
            y += 25;
            gfx.DrawString($"Fecha: {dto.Resumen.Fecha}", fontTexto, XBrushes.Black, new XRect(40, y, page.Width, 20), XStringFormats.TopLeft);
            y += 25;
            gfx.DrawString($"Método de Pago: {dto.Resumen.MetodoPago}", fontTexto, XBrushes.Black, new XRect(40, y, page.Width, 20), XStringFormats.TopLeft);
            y += 25;
            gfx.DrawString($"Método de Entrega: {dto.Resumen.MetodoEntrega}", fontTexto, XBrushes.Black, new XRect(40, y, page.Width, 20), XStringFormats.TopLeft);
            y += 25;
            gfx.DrawString($"Total: S/. {dto.Resumen.Total:F2}", fontTexto, XBrushes.Black, new XRect(40, y, page.Width, 20), XStringFormats.TopLeft);

            using (var stream = new MemoryStream())
            {
                pdf.Save(stream, false);
                return stream.ToArray();
            }
        }
    }
}
