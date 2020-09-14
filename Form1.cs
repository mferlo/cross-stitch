using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Stitcher
{
    public partial class Form : System.Windows.Forms.Form
    {
        Bitmap[] scaledImages;
        Palette palette;

        static readonly object nullColor = new object(); // ObjectCollection throws on null; use this as placeholder value
        static readonly string testImagePath2 = @"C:\Users\Matt\Desktop\stitch\black mage.bmp";

        int ScalingIndex => zoomSlider.Value;
        int ScalingFactor => 1 << zoomSlider.Value;

        public Form()
        {
            InitializeComponent();
            scaledImages = new Bitmap[zoomSlider.Maximum + 1];

            LoadImage(testImagePath2);

            colorListBox.DrawMode = DrawMode.OwnerDrawVariable;
            colorListBox.DrawItem += DrawColorForList;
            canvas.MouseWheel += CanvasMouseWheelHandler;
            ResizeEnd += (_, __) => DrawRulers();
        }

        private void LoadImage(string path)
        {
            zoomSlider.Value = 0;

            for (var i = 0; i < scaledImages.Length; i++)
            {
                scaledImages[i]?.Dispose();
                scaledImages[i] = null;
            }

            palette?.Dispose();

            fileName.Text = path.Split(System.IO.Path.DirectorySeparatorChar).Last();

            var image = new Bitmap(path);
            scaledImages[0] = image;
            AnalyzeImage(image);
            DisplayScaledImage();
        }

        private void AnalyzeImage(Bitmap b)
        {
            dimensionsLabel.Text = $"{b.Height} H, {b.Width} W";

            palette = Palette.FromBitmap(b);

            colorListBox.Items.Clear();
            colorListBox.Items.Add(nullColor);
            colorListBox.Items.AddRange(palette.PaletteInfo.OrderBy(colorInfo => colorInfo.Color.GetHue()).Cast<Object>().ToArray());
        }

        private Bitmap GenerateScaledImage()
        {
            var nativeImage = scaledImages[0];
            var scaledImage = new Bitmap(width: nativeImage.Width * ScalingFactor, height: nativeImage.Height * ScalingFactor);

            /*
             * FIXME: Perf. How to get it to do no anti-aliasing?
            using (var graphics = Graphics.FromImage(scaledImage))
            {
                graphics.SmoothingMode = SmoothingMode.None;
                graphics.ScaleTransform(ScalingFactor, ScalingFactor);
                graphics.DrawImage(nativeImage, 0, 0); //, 0, 0, width: scaledImage.Width, height: scaledImage.Height);
            }
            */

            for (int x = 0; x < nativeImage.Width; x++)
            {
                for (int y = 0; y < nativeImage.Height; y++)
                {
                    var c = nativeImage.GetPixel(x, y);
                    for (int dx = 0; dx < ScalingFactor; dx++)
                    {
                        for (int dy = 0; dy < ScalingFactor; dy++)
                        {
                            scaledImage.SetPixel(ScalingFactor * x + dx, ScalingFactor * y + dy, c);
                        }
                    }
                }
            }

            return scaledImage;
        }

        const int bigTickLength = 5;
        const int bigTickInterval = 10;
        const int littleTickLength = 2;

        private void DrawHeightRuler(int maxHeight)
        {
            heightRuler.Image?.Dispose();

            var image = new Bitmap(width: heightRuler.Width, height: heightRuler.Height);
            using (var graphics = Graphics.FromImage(image))
            {
                graphics.Clear(DefaultBackColor);

                var x1Little = heightRuler.Width - littleTickLength;
                var x1Big = heightRuler.Width - bigTickLength;
                var x2 = heightRuler.Width;
                for (var y = 0; y <= maxHeight; y += ScalingFactor)
                {
                   graphics.DrawLine(Pens.Black, y % bigTickInterval == 0 ? x1Big : x1Little, y, x2, y);
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

                var y1Little = widthRuler.Height - littleTickLength;
                var y1Big = widthRuler.Height - bigTickLength;
                var y2 = widthRuler.Height;
                for (var x = 0; x <= maxWidth; x += ScalingFactor)
                {
                    graphics.DrawLine(Pens.Black, x, x % bigTickInterval == 0 ? y1Big : y1Little, x, y2);
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
            if (scaledImages[ScalingIndex] == null)
            {
                scaledImages[ScalingIndex] = GenerateScaledImage();
            }
            DisplayScaledImage();
        }

        private void DisplayScaledImage()
        {
            DisplayMaybeHighlightedImage(scaledImages[ScalingIndex]);
            DrawRulers();
        }

        private void DisplayMaybeHighlightedImage(Bitmap image)
        {
            if (!scaledImages.Contains(canvas.Image))
            {
                canvas.Image?.Dispose();
            }

            var selectedColor = SelectedColor();

            if (selectedColor == null)
            {
                canvas.Image = image;
                return;
            }

            var highlightedImage = new Bitmap(image);
            var c = selectedColor.Value;
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

        private Color? SelectedColor()
        {
            var selected = colorListBox.SelectedItem;
            if (selected == null || selected == nullColor)
            {
                return null;
            }
            else
            {
                return ((ColorInfo)colorListBox.SelectedItem).Color;
            }
        }

        const int swatchWidth = 20;
        private string B2H(byte b) => b.ToString("X2");
        private string ToHexString(Color c) => $"{B2H(c.R)}{B2H(c.G)}{B2H(c.B)}";

        private string Format(ColorInfo colorInfo, bool isSelected)
        {
            return $"{(isSelected ? "* " : "  ")}{ToHexString(colorInfo.Color)}";

        }

        private void DrawColorForList(object sender, DrawItemEventArgs e)
        {
            var b = e.Bounds;
            var isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            var item = colorListBox.Items[e.Index];

            e.Graphics.FillRectangle(Brushes.White, b);

            if (item is ColorInfo colorInfo)
            {
                e.Graphics.FillRectangle(colorInfo.Brush, b.X, b.Y, b.X + swatchWidth - 1, b.Height);
                e.Graphics.DrawString(Format(colorInfo, isSelected), colorListBox.Font, Brushes.Black, b.X + swatchWidth, b.Y);
            }
            else
            {
                e.Graphics.DrawString(isSelected ? "* ------" : "  ------", colorListBox.Font, Brushes.Black, b.X + swatchWidth, b.Y);
            }
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
            DisplayScaledImage();

        private void zoomSlider_ValueChanged(object sender, EventArgs e) =>
            SetZoom();

        // TODO: drag and drop support
        private void loadButton_Click(object sender, EventArgs e)
        {
            string path;
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.BMP;*.GIF;*.JPG;*.JPEG;*.TIF;*.TIFF;*.PNG";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    path = openFileDialog.FileName;
                }
                else
                {
                    path = null;
                }

            }

            if (path != null)
            {
                LoadImage(path);
            }
        }


        const float monitorPixelsPerInch = 92.0f;
        const float fabricSquaresPerInch = 14.0f;
        const float monitorPixelsPerFabricSquare = monitorPixelsPerInch / fabricSquaresPerInch;

        static readonly string actualSizeCanvasName = "actualSizeCanvas";
        private bool ActualSizeState = false;
        private SaveState ScaledState;

        class SaveState
        {
            public Image WidthRulerImage;
            public Image HeightRulerImage;
            public string DimensionText;
        }

        private (float width, float height) MakeActualSizeImage()
        {
            ScaledState = new SaveState
            {
                WidthRulerImage = widthRuler.Image,
                HeightRulerImage = heightRuler.Image,
                DimensionText = dimensionsLabel.Text,
            };

            var actualSizeCanvas = new PictureBox
            {
                Name = actualSizeCanvasName,
                Location = canvas.Location,
                Size = canvas.Size,
            };

            var originalImage = scaledImages[0];

            var actualWidth = originalImage.Width * monitorPixelsPerFabricSquare;
            var actualHeight = originalImage.Height * monitorPixelsPerFabricSquare;

            actualSizeCanvas.Image = new Bitmap(scaledImages[0], width: (int)actualWidth, height: (int)actualHeight);
            this.Controls.Add(actualSizeCanvas);
            return (width: actualWidth, height: actualHeight);
        }

        private void MakeActualSizeRulers()
        {
            var image = new Bitmap(width: heightRuler.Width, height: heightRuler.Height);
            using (var graphics = Graphics.FromImage(image))
            {
                graphics.Clear(DefaultBackColor);

                var x1Big = heightRuler.Width - bigTickLength;
                var x2 = heightRuler.Width;
                for (var y = 0.0f; y <= heightRuler.Height; y += monitorPixelsPerInch)
                {
                    graphics.DrawLine(Pens.Black, x1Big, y, x2, y);
                }
            }
            heightRuler.Image = image;

            image = new Bitmap(width: widthRuler.Width, height: widthRuler.Height);
            using (var graphics = Graphics.FromImage(image))
            {
                graphics.Clear(DefaultBackColor);

                var y1Big = widthRuler.Height - bigTickLength;
                var y2 = widthRuler.Height;
                for (var x = 0.0f; x <= widthRuler.Width; x += monitorPixelsPerInch)
                {
                    graphics.DrawLine(Pens.Black, x, y1Big, x, y2);
                }
            }

            widthRuler.Image = image;
        }

        private void MakeActualSize()
        {
            ActualSizeState = true;
            actualSize.Text = "Exit";

            var size = MakeActualSizeImage();
            MakeActualSizeRulers();
            dimensionsLabel.Text = $"{Math.Round(size.height / monitorPixelsPerInch, 1)}\" H, {Math.Round(size.width / monitorPixelsPerInch, 1)}\" W";
        }

        private void RestoreScaledSize()
        {
            ActualSizeState = false;
            actualSize.Text = "To Scale";

            var pictureBox = (PictureBox)Controls[actualSizeCanvasName];
            Controls.RemoveByKey(actualSizeCanvasName);
            pictureBox.Image.Dispose();

            widthRuler.Image.Dispose();
            widthRuler.Image = ScaledState.WidthRulerImage;

            heightRuler.Image.Dispose();
            heightRuler.Image = ScaledState.HeightRulerImage;

            dimensionsLabel.Text = ScaledState.DimensionText;
        }

        private void actualSize_Click(object sender, EventArgs e)
        {
            if (scaledImages[0] == null)
            {
                return;
            }

            var otherControls = new List<Control>
            {
                zoomSlider,
                zoomLabel,
                colorListBox,
                loadButton,
                canvas,
            };

            if (ActualSizeState)
            {
                RestoreScaledSize();

                foreach (var c in otherControls)
                {
                    c.Show();
                }
            }
            else
            {
                MakeActualSize();

                foreach (var c in otherControls)
                {
                    c.Hide();
                }
            }
        }
    }
}
