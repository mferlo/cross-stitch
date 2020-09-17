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
        RulerDrawer rulerDrawer;
        Color? selectedBackgroundColor;
        string pixelDimensionString;

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
            ResizeEnd += (_, __) => { InitializeRulerDrawer(); DrawRulers(); };
        }

        private void LoadImage(string path)
        {
            zoomSlider.Value = 0;
            selectedBackgroundColor = null;
            backgroundColor.BackColor = DefaultBackColor;

            for (var i = 0; i < scaledImages.Length; i++)
            {
                scaledImages[i]?.Dispose();
                scaledImages[i] = null;
            }

            palette?.Dispose();

            fileName.Text = path.Split(System.IO.Path.DirectorySeparatorChar).Last();

            var image = new Bitmap(path);
            scaledImages[0] = image;
            pixelDimensionString = $"{image.Height} H, {image.Width} W";
            dimensionsLabel.Text = pixelDimensionString;
            palette = Palette.FromBitmap(image);
            InitializeRulerDrawer();
            InitializeColorListBox();
            DisplayScaledImage();
        }

        private void InitializeRulerDrawer() =>
            rulerDrawer = RulerDrawer.FromComponentSizes(scaledImages[0].Size, heightRuler.Size, widthRuler.Size);

        private void InitializeColorListBox() // FIXME: ObservableCollection?
        {
            colorListBox.Items.Clear();
            colorListBox.Items.Add(nullColor);
            colorListBox.Items.AddRange(palette.PaletteInfo.OrderBy(colorInfo => colorInfo.Color.GetHue()).Cast<Object>().ToArray());

            colorListBox.Height = colorListBox.PreferredHeight;
            setBackgroundButton.Top = colorListBox.Bottom + 6;
            backgroundColor.Top = colorListBox.Bottom + 6;
            setBackgroundButton.Enabled = false;
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

        private void DrawRulers(Size? actualSize = null)
        {
            heightRuler.Image?.Dispose();
            widthRuler.Image?.Dispose();

            (Bitmap heightRuler, Bitmap widthRuler) rulerImages;
            if (actualSize != null)
            {
                rulerImages = rulerDrawer.ActualSize(actualSize.Value);
            }
            else
            {
                rulerImages = rulerDrawer.ToScale(ScalingFactor);
            }

            heightRuler.Image = rulerImages.heightRuler;
            widthRuler.Image = rulerImages.widthRuler;
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
            var backgroundColor = DefaultBackColor;
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

        private string Format(bool isSelected, string color, int count) =>
            $"{(isSelected ? "*" : " ")} {color} ({count,5})";

        private void DrawColorForList(object sender, DrawItemEventArgs e)
        {
            var b = e.Bounds;
            var isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            var item = colorListBox.Items[e.Index];

            e.Graphics.FillRectangle(Brushes.White, b);

            string colorString;
            if (item is ColorInfo colorInfo)
            {
                e.Graphics.FillRectangle(colorInfo.Brush, b.X, b.Y, b.X + swatchWidth - 1, b.Height);
                colorString = Format(isSelected, ToHexString(colorInfo.Color), colorInfo.Count);
            }
            else
            {
                colorString = Format(isSelected, "[All] ", palette.PixelCount);
            }
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

        private void colorListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            setBackgroundButton.Enabled = SelectedColor() != null && selectedBackgroundColor == null;
            DisplayScaledImage();
        }

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

        public const int monitorPixelsPerInch = 92;
        const int fabricSquaresPerInch = 14;
        const float monitorPixelsPerFabricSquare = (float)monitorPixelsPerInch / (float)fabricSquaresPerInch;

        static readonly string actualSizeCanvasName = "actualSizeCanvas";
        private bool ActualSizeState = false;

        private Size MakeActualSizeImage()
        {
            var actualSizeCanvas = new PictureBox
            {
                Name = actualSizeCanvasName,
                Location = canvas.Location,
                Size = canvas.Size,
            };

            var originalImage = scaledImages[0];

            var actualSize = new Size(
                width: (int)(originalImage.Width * monitorPixelsPerFabricSquare),
                height: (int)(originalImage.Height * monitorPixelsPerFabricSquare)
            );

            actualSizeCanvas.Image = new Bitmap(scaledImages[0], actualSize);
            this.Controls.Add(actualSizeCanvas);
            return actualSize;
        }

        private string ActualSizeInInches(Size actualSize)
        {
            var height = (float)actualSize.Height / monitorPixelsPerInch;
            var width = (float)actualSize.Width / monitorPixelsPerInch;
            return $"{Math.Round(height, 1)}\" H, {Math.Round(width, 1)}\" W";
        }

        private void MakeActualSize()
        {
            ActualSizeState = true;
            actualSizeButton.Text = "Exit";

            var actualSize = MakeActualSizeImage();
            DrawRulers(actualSize);
            dimensionsLabel.Text = ActualSizeInInches(actualSize);
        }

        private void RestoreScaledSize()
        {
            ActualSizeState = false;
            actualSizeButton.Text = "To Scale";

            var pictureBox = (PictureBox)Controls[actualSizeCanvasName];
            Controls.RemoveByKey(actualSizeCanvasName);
            pictureBox.Image.Dispose();

            DrawRulers();
            dimensionsLabel.Text = pixelDimensionString;
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

        IEnumerable<(int, int)> EdgeCoordinates(Size size)
        {
            for (var y = 0; y < size.Height; y++)
            {
                yield return (0, y);
                yield return (size.Width - 1, y);
            }

            for (var x = 0; x < size.Width; x++)
            {
                yield return (x, 0);
                yield return (x, size.Height - 1);
            }
        }

        bool ValidPixel(Size size, (int x, int y) p) =>
            0 <= p.x && p.x < size.Width && 0 <= p.y && p.y < size.Height;

        IEnumerable<(int, int)> Neighbors((int x, int y) p)
        {
            yield return (p.x - 1, p.y);
            yield return (p.x + 1, p.y);
            yield return (p.x, p.y - 1);
            yield return (p.x, p.y + 1);
        }

        IEnumerable<(int, int)> ValidNeighbors(Size size, (int, int) pixel) =>
            Neighbors(pixel).Where(p => ValidPixel(size, p));

        private int RemoveBackgroundColor(Color color)
        {
            var image = scaledImages[0];

            var backgroundPixels = new HashSet<(int x, int y)>();
            var pixelsToExamine = new Queue<(int x, int y)>(EdgeCoordinates(image.Size));

            while (pixelsToExamine.Any())
            {
                var p = pixelsToExamine.Dequeue();
                var c = image.GetPixel(p.x, p.y);
                if (c == color)
                {
                    backgroundPixels.Add(p);
                    foreach (var newPixel in ValidNeighbors(image.Size, p))
                    {
                        if (!pixelsToExamine.Contains(newPixel) && !backgroundPixels.Contains(newPixel))
                        {
                            pixelsToExamine.Enqueue(newPixel);
                        }
                    }
                }
            }

            foreach (var p in backgroundPixels)
            {
                image.SetPixel(p.x, p.y, DefaultBackColor);
            }

            return backgroundPixels.Count;
        }

        private void setBackgroundButton_Click(object sender, EventArgs e)
        {
            var bgColor = SelectedColor().Value;
            backgroundColor.BackColor = bgColor;
            selectedBackgroundColor = bgColor;

            var pixelsRemoved = RemoveBackgroundColor(bgColor);
            this.palette.Remove(bgColor, pixelsRemoved);
            InitializeColorListBox();

            // Get rid of the old scaled images
            var oldZoom = zoomSlider.Value;
            zoomSlider.Value = 0;
            for (var i = 1; i < scaledImages.Length; i++)
            {
                scaledImages[i]?.Dispose();
                scaledImages[i] = null;
            }
            zoomSlider.Value = oldZoom;
            DisplayScaledImage();

            // Undo logic isn't worth doing. User can reload the file to undo/change the bg color
            setBackgroundButton.Enabled = false; 
        }
    }
}
