using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using MoreLinq;

namespace Stitcher
{
    static class ColorHelper
    {
        static readonly Dictionary<RGB, string> ColorNames;

        public static string NameOf(Color color) =>
            ClosestColor(new RGB(color));

        static string ClosestColor(RGB rgb) =>
            ColorNames[ColorNames.Keys.MinBy(c => rgb.EuclideanDistance(c)).First()];


        readonly struct RGB
        {
            public RGB(Color c)
            {
                R = c.R;
                G = c.G;
                B = c.B;
            }

            public int EuclideanDistance(RGB other) =>
                (this.R - other.R) * (this.R - other.R) +
                (this.G - other.G) * (this.G - other.G) +
                (this.B - other.B) * (this.B - other.B);

            public byte R { get; }
            public byte G { get; }
            public byte B { get; }
        }

        static ColorHelper()
        {
            const byte fullyOpaque = 255;

            ColorNames = new Dictionary<RGB, string>();

            // You'd _think_ the KnownColors Enum would be helpful here,
            // but it also knows about UI element colors, and prefers to
            // return things like "ButtonHighlight" instead of "White".

            var colorProperties = typeof(Color).GetProperties(BindingFlags.Static | BindingFlags.Public)
                .Where(pi => pi.PropertyType == typeof(Color));

            foreach (var prop in colorProperties)
            {
                var color = (Color)prop.GetValue(null);
                if (color.A != fullyOpaque)
                {
                    continue;
                }

                var rgb = new RGB(color);
                var name = Regex.Replace(prop.Name, "([a-z])([A-Z])", "$1 $2");

                ColorNames[rgb] = name;
            }
        }
    }
}