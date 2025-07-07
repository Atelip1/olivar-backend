using PdfSharpCore.Fonts;
using System.Reflection;
using System.IO;

public class CustomFontResolver : IFontResolver
{
    public string DefaultFontName => "Verdana";

    public byte[] GetFont(string faceName)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var resourceName = faceName switch
        {
            "Verdana#Regular" => "OlivarBackend.Fonts.verdana.ttf",
            "Verdana#Bold" => "OlivarBackend.Fonts.verdanab.ttf",
            "Verdana#Italic" => "OlivarBackend.Fonts.verdanai.ttf",
            "Verdana#BoldItalic" => "OlivarBackend.Fonts.verdanaz.ttf",
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
            if (isBold && isItalic)
                return new FontResolverInfo("Verdana#BoldItalic");
            else if (isBold)
                return new FontResolverInfo("Verdana#Bold");
            else if (isItalic)
                return new FontResolverInfo("Verdana#Italic");
            else
                return new FontResolverInfo("Verdana#Regular");
        }

        return new FontResolverInfo("Verdana#Regular");
    }
}
