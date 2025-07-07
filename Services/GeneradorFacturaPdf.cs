using OlivarBackend.Dto;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System.IO;

public class GeneradorFacturaPdf
{
    public static byte[] GenerarFacturaPdf(EmailFacturaDto dto)
    {
        var pdf = new PdfDocument();
        var page = pdf.AddPage();
        var gfx = XGraphics.FromPdfPage(page);

        XFont font = new XFont("Arial", 14, XFontStyle.Regular);

        double y = 40;
        gfx.DrawString("Factura de Compra", font, XBrushes.Black,
            new XRect(0, y, page.Width, 30), XStringFormats.TopCenter);

        y += 40;
        gfx.DrawString($"Correo: {dto.Email}", font, XBrushes.Black, new XRect(40, y, page.Width, 20), XStringFormats.TopLeft);
        y += 20;
        gfx.DrawString($"Fecha: {dto.Resumen?.Fecha}", font, XBrushes.Black, new XRect(40, y, page.Width, 20), XStringFormats.TopLeft);
        y += 20;
        gfx.DrawString($"Método de Pago: {dto.Resumen?.MetodoPago}", font, XBrushes.Black, new XRect(40, y, page.Width, 20), XStringFormats.TopLeft);
        y += 20;
        gfx.DrawString($"Método de Entrega: {dto.Resumen?.MetodoEntrega}", font, XBrushes.Black, new XRect(40, y, page.Width, 20), XStringFormats.TopLeft);
        y += 20;
        gfx.DrawString($"Total: S/. {dto.Resumen?.Total:F2}", font, XBrushes.Black, new XRect(40, y, page.Width, 20), XStringFormats.TopLeft);

        using var stream = new MemoryStream();
        pdf.Save(stream, false);
        return stream.ToArray();
    }
}
