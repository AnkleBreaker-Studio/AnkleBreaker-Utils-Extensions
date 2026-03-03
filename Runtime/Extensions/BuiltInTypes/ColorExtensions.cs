using UnityEngine;

namespace AnkleBreaker.Utils.Extensions
{
    public static class ColorExtensions
    {
        public static Color SetAlpha(this Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }

        public static Color WithR(this Color color, float r)
        {
            return new Color(r, color.g, color.b, color.a);
        }

        public static Color WithG(this Color color, float g)
        {
            return new Color(color.r, g, color.b, color.a);
        }

        public static Color WithB(this Color color, float b)
        {
            return new Color(color.r, color.g, b, color.a);
        }

        /// <summary>
        /// Converts a Color to its hexadecimal string representation.
        /// </summary>
        /// <param name="color">The color to convert.</param>
        /// <param name="includeAlpha">If true, returns "#RRGGBBAA", otherwise "#RRGGBB".</param>
        public static string ToHexString(this Color color, bool includeAlpha = false)
        {
            Color32 c32 = color;
            return includeAlpha
                ? $"#{c32.r:X2}{c32.g:X2}{c32.b:X2}{c32.a:X2}"
                : $"#{c32.r:X2}{c32.g:X2}{c32.b:X2}";
        }
    }
}
