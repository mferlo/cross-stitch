using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Stitcher
{
    // FIXME: note every 10 ticks; add markers pointing to center of grid
    // FIXME: try to name colors
    // FIXME: automatically compose pieces for best printing
    // FIXME: more grid symbols

    static class Printer
    {
        private static readonly Font font = new Font("Calibri", 12);

        // Each square is 20x20. There is a 1-pixel line between each grid
        const int gridSquareSize = 20;
        const int gridLineSize = 1;

        // Double fencepost problem:
        // 1. Each pixel in the image is gridSize x gridSize, but it also has a 1 pixel border
        // 2. We want to display a border on each edge of the image as a whole
        private static int ToPrintCoord(int i) => (i + 1) * gridLineSize + i * gridSquareSize;

        private delegate void PrintSymbol(Graphics graphics, int x, int y);

        public static Image CreatePrintableImage(Bitmap image, Palette palette)
        {
            var symbolPrinters = GetSymbolPrinters(palette);

            var stitchGridImage = CreateStitchGridImage(image, symbolPrinters);
            var colorInfoImage = CreateColorInfoImage(symbolPrinters, palette);

            var spacing = 20;

            // FIXME: compose pieces best printing (e.g., if wide, make landscape)
            var result = new Bitmap(
                width: Math.Max(stitchGridImage.Width, colorInfoImage.Width),
                height: stitchGridImage.Height + colorInfoImage.Height + spacing
            );

            using (var graphics = Graphics.FromImage(result))
            {
                graphics.DrawImage(stitchGridImage, 0, 0);
                graphics.DrawImage(colorInfoImage, 0, stitchGridImage.Height + spacing);
            }

            return result;
        }

        private static Bitmap CreateStitchGridImage(Bitmap image, Dictionary<Color, PrintSymbol> symbolPrinters)
        {
            var result = new Bitmap(
                width: image.Width * (gridSquareSize + 1) + 1,
                height: image.Height * (gridSquareSize + 1) + 1
            );

            using (var graphics = Graphics.FromImage(result))
            {
                graphics.FillRectangle(Brushes.White, 0, 0, result.Width, result.Height);
                DrawGrid(graphics, result.Width, result.Height);

                for (var x = 0; x < image.Width; x++)
                {
                    for (var y = 0; y < image.Height; y++)
                    {
                        var pixel = image.GetPixel(x, y);

                        // FIXME: better way to deal with background color
                        if (symbolPrinters.TryGetValue(pixel, out var sp))
                        {
                            sp(graphics, ToPrintCoord(x), ToPrintCoord(y));
                        }
                    }
                }
            }

            return result;
        }

        private static Bitmap CreateColorInfoImage(Dictionary<Color, PrintSymbol> symbolPrinters, Palette palette)
        {
            var result = new Bitmap(
                width: 300,
                height: (gridSquareSize + 1) * symbolPrinters.Count
            );

            var y = 0;

            using (var graphics = Graphics.FromImage(result))
            {
                foreach (var colorInfo in palette.PaletteInfo.OrderByDescending(pi => pi.Count))
                {
                    var sp = symbolPrinters[colorInfo.Color];
                    sp(graphics, 0, y);

                    var colorString = $"{ColorName(colorInfo.Color)} ({colorInfo.Count})";

                    graphics.DrawString(colorString, font, Brushes.Black, gridSquareSize + 5, y);

                    y += gridSquareSize + 1;
                }
            }

            return result;
        }

        private static string ColorName(Color c)
        {
            // FIXME: get actual name (or closest known name)
            string B2H(byte b) => b.ToString("X2");
            return $"{B2H(c.R)}{B2H(c.G)}{B2H(c.B)}";
        }

        private static void DrawGrid(Graphics g, int width, int height)
        {
            for (var x = 0; x <= width; x += gridSquareSize + gridLineSize)
            {
                g.DrawLine(Pens.Black, x, 0, x, height);
            }

            for (var y = 0; y <= height; y += gridSquareSize + gridLineSize)
            {
                g.DrawLine(Pens.Black, 0, y, width, y);
            }
        }

        private static readonly IReadOnlyList<PrintSymbol> Printers =
            new List<PrintSymbol>() { PrintX, PrintTriangle, PrintCircle, PrintCross, PrintDiamond };

        private static Dictionary<Color, PrintSymbol> GetSymbolPrinters(Palette palette)
        {
            var result = new Dictionary<Color, PrintSymbol>();

            var colors = palette.PaletteInfo.OrderByDescending(pi => pi.Count).Select(pi => pi.Color).ToList();

            var printer = 0;
            foreach (var color in colors.Take(Printers.Count))
            {
                result[color] = Printers[printer++];
            }

            printer = 1;
            foreach (var color in colors.Skip(Printers.Count))
            {
                result[color] = MakePrinter(printer++);
            }

            return result;
        }

        private const int margin = 5;

        private static int Left(int x) => x + margin;
        private static int Right(int x) => x + gridSquareSize - margin + 1;
        private static int Top(int y) => y + margin;
        private static int Bottom(int y) => y + gridSquareSize - margin + 1;
        private static int Mid(int z) => z + gridSquareSize / 2;

        private static Pen BlackWide = new Pen(Color.Black, 3);

        private static void PrintX(Graphics graphics, int x, int y)
        {
            graphics.DrawLine(BlackWide, Left(x), Top(y), Right(x), Bottom(y));
            graphics.DrawLine(BlackWide, Left(x), Bottom(y), Right(x), Top(y));
        }

        private static void PrintTriangle(Graphics graphics, int x, int y)
        {
            graphics.FillRectangle(Brushes.Black, x, y, gridSquareSize, gridSquareSize);
            var top = new Point(Mid(x), Top(y));
            var left = new Point(Left(x), Bottom(y));
            var right = new Point(Right(x), Bottom(y));
            graphics.FillPolygon(Brushes.White, new[] { top, left, right });
        }

        private static void PrintCircle(Graphics graphics, int x, int y)
        {
            graphics.FillEllipse(Brushes.Black, Left(x), Top(y), gridSquareSize - 2 * margin, gridSquareSize - 2 * margin);
        }

        private static void PrintCross(Graphics graphics, int x, int y)
        {
            graphics.DrawLine(BlackWide, Mid(x), Top(y), Mid(x), Bottom(y));
            graphics.DrawLine(BlackWide, Left(x), Mid(y), Right(x), Mid(y));
        }

        private static void PrintDiamond(Graphics graphics, int x, int y)
        {
            var top = new Point(Mid(x), Top(y));
            var bottom = new Point(Mid(x), Bottom(y));
            var left = new Point(Left(x), Mid(y));
            var right = new Point(Right(x), Mid(y));
            graphics.FillPolygon(Brushes.Black, new[] { top, right, bottom, left });
        }

        // Note: Deliberately not using Left(x) or Top(y) because it makes number too far right/down
        private static PrintSymbol MakePrinter(int symbol) =>
            (Graphics Graphics, int x, int y) =>
                Graphics.DrawString(symbol.ToString(), font, Brushes.Black, x, y);
    }
}