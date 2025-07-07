using PdfSharpCore.Fonts;
using System.Reflection;
using System.IO;

public class CustomFontResolver : IFontResolver
{
    // 🔥 Propiedad obligatoria que faltaba
    public string DefaultFontName => "Verdana";

    public byte[] GetFont(string faceName)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var resourceName = faceName switch
        {
            "Verdana#Regular" => "OlivarBackend.Fonts.verdana.ttf",
            "Verdana#Bold" => "OlivarBackend.Fonts.verdanab.ttf",
            _ => throw new InvalidOperationException($"No se encontró la fuente para {faceName}")
        };

        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"No se pudo cargar la fuente embebida {resourceName}");

        using var ms = new MemoryStream();
        stream.CopyTo(ms);
        return ms.ToArray();
    }

    public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
    {
        if (familyName.Equals("Verdana", StringComparison.OrdinalIgnoreCase))
        {
            return isBold
                ? new FontResolverInfo("Verdana#Bold")
                : new FontResolverInfo("Verdana#Regular");
        }

        // Fallback
        return new FontResolverInfo("Verdana#Regular");
    }
}
