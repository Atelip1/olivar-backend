using PdfSharp.Fonts;
using System.Reflection;

public class CustomFontResolver : IFontResolver
{
    public byte[] GetFont(string faceName)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var resourceName = faceName switch
        {
            "Verdana#Regular" => "OlivarBackend.Fonts.verdana.ttf",
            "Verdana#Bold" => "OlivarBackend.Fonts.verdanab.ttf",
            _ => throw new InvalidOperationException($"No font defined for {faceName}")
        };

        using var stream = assembly.GetManifestResourceStream(resourceName);
        if (stream == null)
            throw new InvalidOperationException("No se pudo cargar la fuente " + resourceName);

        using var ms = new MemoryStream();
        stream.CopyTo(ms);
        return ms.ToArray();
    }

    public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
    {
        if (familyName.Equals("Verdana", StringComparison.OrdinalIgnoreCase))
        {
            if (isBold)
                return new FontResolverInfo("Verdana#Bold");
            else
                return new FontResolverInfo("Verdana#Regular");
        }

        return PlatformFontResolver.ResolveTypeface(familyName, isBold, isItalic);
    }
}
