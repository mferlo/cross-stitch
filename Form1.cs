using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Stitcher
{
    public partial class Form : System.Windows.Forms.Form
    {
        Bitmap[] scaledImages;
        Dictionary<Color, Brush> brushes = new Dictionary<Color, Brush>();

        static readonly object nullColor = new object(); // ObjectCollection throws on null; use this as placeholder value
        static readonly string testImagePath = @"C:\Users\Matt\Desktop\stitch\shane.bmp";

        Color? SelectedColor => colorListBox.SelectedItem == nullColor ? null : (Color?)colorListBox.SelectedItem;
        int ScalingIndex => zoomSlider.Value;
        int ScalingFactor => 1 << zoomSlider.Value;

        public Form()
        {
            InitializeComponent();

            scaledImages = new Bitmap[zoomSlider.Maximum + 1];
            brushes = new Dictionary<Color, Brush>();

            LoadImage(testImagePath);

            colorListBox.DrawMode = DrawMode.OwnerDrawVariable;
            colorListBox.DrawItem += DrawColorForList;
            canvas.MouseWheel += CanvasMouseWheelHandler;
            ResizeEnd += (_, __) => DrawRulers();
        }

        private void LoadImage(string path)
        {
            foreach (var oldImage in scaledImages.Where(i => i != null))
            {
                oldImage.Dispose();
            }

            foreach (var b in brushes.Values)
            {
                b.Dispose();
            }

            fileName.Text = path.Split(System.IO.Path.DirectorySeparatorChar).Last();

            using (var image = new Bitmap(path))
            {
                AnalyzeColors(image);
                dimensionsLabel.Text = $"{image.Height}, {image.Width}";
                GenerateScaledImages(image);
            }

            SetZoom();
        }

        private void AnalyzeColors(Bitmap b)
        {
            var colors = new HashSet<Color>();
            for (int x = 0; x < b.Width; x++)
            {
                for (int y = 0; y < b.Height; y++)
                {
                    colors.Add(b.GetPixel(x, y));
                }
            }
            brushes = colors.ToDictionary(c => c, c => (Brush)new SolidBrush(c));

            var colorList = colors.OrderBy(c => c.GetHue()).Cast<object>().ToArray();
            colorListBox.Items.Clear();
            colorListBox.Items.Add(nullColor);
            colorListBox.Items.AddRange(colorList);
            colorListBox.SelectedIndex = 0;
        }

        private Bitmap GenerateScaledImage(Bitmap nativeImage, int scalingIndex)
        {
            var scalingFactor = 1 << scalingIndex;

            var scaledImage = new Bitmap(width: nativeImage.Width * scalingFactor, height: nativeImage.Height * scalingFactor);
            for (int x = 0; x < nativeImage.Width; x++)
            {
                for (int y = 0; y < nativeImage.Height; y++)
                {
                    var c = nativeImage.GetPixel(x, y);
                    for (int dx = 0; dx < scalingFactor; dx++)
                    {
                        for (int dy = 0; dy < scalingFactor; dy++)
                        {
                            scaledImage.SetPixel(scalingFactor * x + dx, scalingFactor * y + dy, c);
                        }
                    }
                }
            }

            return scaledImage;
        }

        private void GenerateScaledImages(Bitmap nativeImage)
        {
            for (int i = 0; i <= zoomSlider.Maximum; i++)
            {
                scaledImages[i] = GenerateScaledImage(nativeImage, i);
            }
        }

        const int rulerBigTickLength = 5;
        const int rulerSmallTickLength = 2;
        private void DrawHeightRuler(int maxHeight)
        {
            heightRuler.Image?.Dispose();

            var image = new Bitmap(width: heightRuler.Width, height: heightRuler.Height);
            using (var graphics = Graphics.FromImage(image))
            {
                graphics.Clear(DefaultBackColor);

                var x1 = heightRuler.Width - rulerBigTickLength;
                var x2 = heightRuler.Width;
                for (var y = 0; y < maxHeight; y += 10 * ScalingFactor)
                {
                    graphics.DrawLine(Pens.Black, x1, y, x2, y);
                }

                if (ScalingFactor > 1)
                {
                    x1 = heightRuler.Width - rulerSmallTickLength;
                    for (var y = 0; y < maxHeight; y += ScalingFactor)
                    {
                        graphics.DrawLine(Pens.Black, x1, y, x2, y);
                    }
                }
            }

            heightRuler.Image = image;
        }

        private void DrawWidthRuler(int maxWidth)
        {
            widthRuler.Image?.Dispose();

            var image = new Bitmap(width: widthRuler.Width, height: widthRuler.Height);
            using (var graphics = Graphics.FromImage(image))
            {
                graphics.Clear(DefaultBackColor);

                var y1 = widthRuler.Height - rulerBigTickLength;
                var y2 = widthRuler.Height;
                for (var x = 0; x < maxWidth; x += 10 * ScalingFactor)
                {
                    graphics.DrawLine(Pens.Black, x, y1, x, y2);
                }

                if (ScalingFactor > 1)
                {
                    y1 = widthRuler.Height - rulerSmallTickLength;
                    for (var x = 0; x < maxWidth; x += ScalingFactor)
                    {
                        graphics.DrawLine(Pens.Black, x, y1, x, y2);
                    }
                }
            }

            widthRuler.Image = image;
        }

        private void DrawRulers()
        {
            if (canvas.Image != null)
            {
                DrawHeightRuler(canvas.Image.Height);
                DrawWidthRuler(canvas.Image.Width);
            }
        }

        private void SetZoom()
        {
            zoomLabel.Text = $"{100 * (1 << ScalingIndex)}%";
            Redraw();
        }

        private void Redraw()
        {
            SetDisplayImage(scaledImages[ScalingIndex]);
            DrawRulers();
        }

        private void SetDisplayImage(Bitmap image)
        {
            if (!scaledImages.Contains(canvas.Image))
            {
                canvas.Image?.Dispose();
            }

            if (SelectedColor == null)
            {
                canvas.Image = image;
                return;
            }

            var highlightedImage = new Bitmap(image);
            var c = SelectedColor.Value;
            var backgroundColor = c.GetBrightness() > 0.9 ? Color.Black : Color.White;
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    if (image.GetPixel(x, y) != c)
                    {
                        highlightedImage.SetPixel(x, y, backgroundColor);
                    }
                }
            }

            canvas.Image = highlightedImage;
        }


        // Color List Support
        // TODO: Allow multi-select

        const int swatchWidth = 20;
        private string B2H(byte b) => b.ToString("X2");
        private string ToHexString(Color c) => $"{B2H(c.R)}{B2H(c.G)}{B2H(c.B)}";

        private void DrawColorForList(object sender, DrawItemEventArgs e)
        {
            var b = e.Bounds;
            var isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            var item = colorListBox.Items[e.Index];

            if (item == nullColor)
            {
                e.Graphics.FillRectangle(Brushes.White, b);
                e.Graphics.DrawString(isSelected ? "* ------" : "  ------", colorListBox.Font, Brushes.Black, b.X + swatchWidth, b.Y);
                return;
            }

            var color = (Color)item;

            e.Graphics.FillRectangle(brushes[color], b.X, b.Y, b.X + swatchWidth - 1, b.Height);
            e.Graphics.FillRectangle(Brushes.White, b.X + swatchWidth, b.Y, b.Width - swatchWidth, b.Height);

            var colorString = $"{(isSelected ? "* " : "  ")}{ToHexString(color)}";
            e.Graphics.DrawString(colorString, colorListBox.Font, Brushes.Black, b.X + swatchWidth, b.Y);
        }

        private void CanvasMouseWheelHandler(object sender, MouseEventArgs e)
        {
            var result = zoomSlider.Value + e.Delta / 120;
            if (zoomSlider.Minimum <= result && result <= zoomSlider.Maximum)
            {
                zoomSlider.Value = result;
            }
        }

        private void colorListBox_SelectedIndexChanged(object sender, EventArgs e) =>
            Redraw();

        private void zoomSlider_ValueChanged(object sender, EventArgs e) =>
            SetZoom();

        // TODO: actual load dialog, drag and drop support
        private void loadButton_Click(object sender, EventArgs e) =>
            LoadImage(testImagePath);
    }
}
