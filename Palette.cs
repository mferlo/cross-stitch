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
        public void Dec(int x) => Count -= x;
    }

    public class Palette : IDisposable
    {
        public IEnumerable<ColorInfo> PaletteInfo => palette.Values;
        public int PixelCount { get; private set; }

        private Dictionary<Color, ColorInfo> palette;

        private Palette(Dictionary<Color, ColorInfo> palette)
        {
            this.palette = palette;
            PixelCount = this.palette.Values.Sum(ci => ci.Count);
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

            return new Palette(data);
        }

        public void Remove(Color color, int count)
        {
            this.PixelCount -= count;

            var removedColorInfo = palette[color];
            if (removedColorInfo.Count == count)
            {
                palette.Remove(color);
                removedColorInfo.Brush.Dispose();
            }
            else
            {
                removedColorInfo.Dec(count);
            }
        }

        public void Dispose()
        {
            foreach (var brush in palette.Values.Select(i => i.Brush))
            {
                brush.Dispose();
            }
        }
    }

}
