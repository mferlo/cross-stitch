using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;


namespace Stitcher
{
    public class ColorInfo
    {
        public Color Color { get;  }
        public Brush Brush { get; }
        public int Count { get; private set; }

        public ColorInfo(Color color)
        {
            Color = color;
            Brush = new SolidBrush(color);
            Count = 1;
        }

        public void Inc() => Count++;
    }

    public class Palette : IDisposable
    {
        public IEnumerable<ColorInfo> PaletteInfo => palette;
        public int PixelCount { get; }

        private List<ColorInfo> palette;

        private Palette(List<ColorInfo> palette)
        {
            this.palette = palette;
            PixelCount = palette.Sum(ci => ci.Count);
        }

        public static Palette FromBitmap(Bitmap bitmap)
        {
            var data = new Dictionary<Color, ColorInfo>();

            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    var color = bitmap.GetPixel(x, y);
                    if (data.TryGetValue(color, out var colorInfo))
                    {
                        colorInfo.Inc();
                    }
                    else
                    {
                        data.Add(color, new ColorInfo(color));
                    }
                }
            }

            return new Palette(data.Values.ToList());
        }

        public void Dispose()
        {
            foreach (var brush in palette.Select(i => i.Brush))
            {
                brush.Dispose();
            }
        }
    }

}
