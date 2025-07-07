using PdfSharp.Fonts;
using System.Reflection;

public class CustomFontResolver : IFontResolver
{
    public byte[] GetFont(string faceName)
    {
        var fontName = faceName.ToLowerInvariant();

        return fontName switch
        {
            "verdana" => LoadFontData("Fonts.verdana.ttf"),
            "verdanab" => LoadFontData("Fonts.verdanab.ttf"),
            "verdanai" => LoadFontData("Fonts.verdanai.ttf"),
            "verdanaz" => LoadFontData("Fonts.verdanaz.ttf"),
            _ => throw new InvalidOperationException($"Fuente no encontrada: {faceName}")
        };
    }

    public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
    {
        if (familyName.ToLowerInvariant() == "verdana")
        {
            if (isBold && isItalic)
                return new FontResolverInfo("verdanaz");
            if (isBold)
                return new FontResolverInfo("verdanab");
            if (isItalic)
                return new FontResolverInfo("verdanai");

            return new FontResolverInfo("verdana");
        }

        throw new InvalidOperationException($"Fuente no soportada: {familyName}");
    }

    private static byte[] LoadFontData(string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"No se encontró la fuente embebida: {resourceName}");
        using var ms = new MemoryStream();
        stream.CopyTo(ms);
        return ms.ToArray();
    }
}
