using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace Stitcher
{
    public class Palette : IDisposable
    {
        class ColorInfo
        {
            public int Count;
            public Brush Brush;
        }

        private Dictionary<Color, ColorInfo> palette;

        public static Palette FromBitmap(Bitmap bitmap)
        {
            var palette = new Dictionary<Color, ColorInfo>();

            var colorCounts = new Dictionary<Color, int>();
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    var color = bitmap.GetPixel(x, y);
                    if (palette.TryGetValue(color, out var colorInfo))
                    {
                        colorInfo.Count++;
                    }
                    else
                    {
                        palette.Add(color, new ColorInfo { Brush = new SolidBrush(color), Count = 1 });
                    }
                }
            }

            return new Palette { palette = palette };
        }

        public IEnumerable<Color> Colors => palette.Keys.OrderBy(c => c.GetHue());
        public Brush Brush(Color color) => palette[color].Brush;
        public int Count(Color color) => palette[color].Count;

        public void Dispose()
        {
            foreach (var brush in palette.Values.Select(i => i.Brush))
            {
                brush.Dispose();
            }
        }
    }

}
