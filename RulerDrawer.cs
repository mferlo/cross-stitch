using System;
using System.Drawing;

namespace Stitcher
{
    class RulerDrawer
    {
        private const int bigTickLength = 5;
        private const int bigTickInterval = 10;
        private const int littleTickLength = 2;

        private const int PPI = Form.monitorPixelsPerInch;

        private static readonly Color backgroundColor = Color.Transparent;

        private Size heightRuler;
        private Size widthRuler;
        private Size unscaledImage;

        private RulerDrawer()
        {}

        public static RulerDrawer FromComponentSizes(Size unscaledImage, Size heightRuler, Size widthRuler) =>
            new RulerDrawer
            {
                unscaledImage = unscaledImage,
                heightRuler = heightRuler,
                widthRuler = widthRuler
            };
        
        public (Bitmap heightRuler, Bitmap widthRuler) ToScale(int scalingFactor) =>
            (DrawHeightRuler(scalingFactor), DrawWidthRuler(scalingFactor));

        public (Bitmap heightRuler, Bitmap widthRuler) ActualSize(Size actualSize)
        {
            var heightImage = new Bitmap(width: heightRuler.Width, height: heightRuler.Height);
            using (var graphics = Graphics.FromImage(heightImage))
            {
                graphics.Clear(backgroundColor);

                var x1Big = heightRuler.Width - bigTickLength;
                var x2 = heightRuler.Width;
                for (var y = 0; y <= actualSize.Height + PPI; y += PPI)
                {
                    graphics.DrawLine(Pens.Black, x1Big, y, x2, y);
                }
            }

            var widthImage = new Bitmap(width: widthRuler.Width, height: widthRuler.Height);
            using (var graphics = Graphics.FromImage(widthImage))
            {
                graphics.Clear(backgroundColor);

                var y1Big = widthRuler.Height - bigTickLength;
                var y2 = widthRuler.Height;
                for (var x = 0; x <= actualSize.Width + PPI; x += PPI)
                {
                    graphics.DrawLine(Pens.Black, x, y1Big, x, y2);
                }
            }

            return (heightImage, widthImage);
        }

        private Bitmap DrawGrid(int scalingFactor)
        {
            var image = new Bitmap(width: unscaledImage.Width * scalingFactor, height: unscaledImage.Height * scalingFactor);
            using (var graphics = Graphics.FromImage(image))
            {
                graphics.Clear(Color.Transparent);
                graphics.DrawLine(Pens.Black, 0, 0, 42, 42);
            }
            return image;
        }


        private Bitmap DrawHeightRuler(int scalingFactor)
        {
            var image = new Bitmap(width: heightRuler.Width, height: heightRuler.Height);
            using (var graphics = Graphics.FromImage(image))
            {
                graphics.Clear(backgroundColor);

                var x1Little = heightRuler.Width - littleTickLength;
                var x1Big = heightRuler.Width - bigTickLength;
                var x2 = heightRuler.Width;
                for (var y = 0; y <= unscaledImage.Height * scalingFactor; y += scalingFactor)
                {
                   graphics.DrawLine(Pens.Black, y % bigTickInterval == 0 ? x1Big : x1Little, y, x2, y);
                }
            }

            return image;
        }

        private Bitmap DrawWidthRuler(int scalingFactor)
        {
            var image = new Bitmap(width: widthRuler.Width, height: widthRuler.Height);
            using (var graphics = Graphics.FromImage(image))
            {
                graphics.Clear(backgroundColor);

                var y1Little = widthRuler.Height - littleTickLength;
                var y1Big = widthRuler.Height - bigTickLength;
                var y2 = widthRuler.Height;
                for (var x = 0; x <= unscaledImage.Width * scalingFactor; x += scalingFactor)
                {
                    graphics.DrawLine(Pens.Black, x, x % bigTickInterval == 0 ? y1Big : y1Little, x, y2);
                }
            }

            return image;
        }
    }
}
