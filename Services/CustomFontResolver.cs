using PdfSharp.Fonts;
using System;
using System.IO;

namespace OlivarBackend.Services
{
    public class CustomFontResolver : IFontResolver
    {
        private static readonly string FontFolder = Path.Combine(Directory.GetCurrentDirectory(), "Fonts");

        public byte[] GetFont(string faceName)
        {
            string fontPath = faceName switch
            {
                "Verdana#Regular" => Path.Combine(FontFolder, "verdana.ttf"),
                "Verdana#Bold" => Path.Combine(FontFolder, "verdanab.ttf"),
                "Verdana#Italic" => Path.Combine(FontFolder, "verdanai.ttf"),
                "Verdana#BoldItalic" => Path.Combine(FontFolder, "verdanaz.ttf"),
                _ => throw new ArgumentException($"Fuente no soportada: {faceName}")
            };

            return File.ReadAllBytes(fontPath);
        }

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            if (string.Equals(familyName, "Verdana", StringComparison.OrdinalIgnoreCase))
            {
                if (isBold && isItalic)
                    return new FontResolverInfo("Verdana#BoldItalic");
                if (isBold)
                    return new FontResolverInfo("Verdana#Bold");
                if (isItalic)
                    return new FontResolverInfo("Verdana#Italic");
                return new FontResolverInfo("Verdana#Regular");
            }

            // Si no se encuentra, se puede lanzar una excepción o usar fuente por defecto
            return PlatformFontResolver.ResolveTypeface("Arial", isBold, isItalic);
        }

        public string DefaultFontName => "Verdana";
    }
}
