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
        int scalingFactor;
        Bitmap image;
        Bitmap displayImage;
        Dictionary<Color, Brush> brushes;

        public Form()
        {
            InitializeComponent();
            scalingFactor = 1;
            loadButton_Click(null, null);

            colorListBox.DrawMode = DrawMode.OwnerDrawVariable;
            colorListBox.DrawItem += new DrawItemEventHandler(drawColorForList);
            // colorListBox.MeasureItem += new MeasureItemEventHandler(measureColorForList);
        }

        /*
         TODO:
            highlight colors in image on selection
         */
        const int swatchWidth = 20;
        private void drawColorForList(object sender, DrawItemEventArgs e)
        {
            var b = e.Bounds;

            var isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            var color = (Color)colorListBox.Items[e.Index];

            e.Graphics.FillRectangle(brushes[color], b.X, b.Y, b.X + swatchWidth - 1, b.Height);
            e.Graphics.FillRectangle(Brushes.White, b.X + swatchWidth, b.Y, b.Width - swatchWidth, b.Height);

            var colorString = $"{ToHexString(color)}{(isSelected ? " *" : "  ")}";
            e.Graphics.DrawString(colorString, colorListBox.Font, Brushes.Black, b.X + swatchWidth, b.Y);
        }

        private string B2H(byte b) => b.ToString("X2");
        private string ToHexString(Color c) => $"{B2H(c.R)}{B2H(c.G)}{B2H(c.B)}";

        private void AnalyzeColors()
        {
            var colors = new HashSet<Color>();
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    colors.Add(image.GetPixel(x, y));
                }
            }
            brushes = colors.ToDictionary(c => c, c => (Brush)new SolidBrush(c));
            colorListBox.Items.AddRange(brushes.Keys.OrderBy(c => c.GetHue()).Cast<object>().ToArray());
        }

        // TODO: display image size & grid marks along outside
        private void SetDisplayImage()
        {
            if (displayImage != null)
            {
                displayImage.Dispose();
            }

            displayImage = new Bitmap(width: image.Width * scalingFactor, height: image.Height * scalingFactor);
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    var c = image.GetPixel(x, y);
                    for (int dx = 0; dx < scalingFactor; dx++)
                    {
                        for (int dy = 0; dy < scalingFactor; dy++)
                        {
                            displayImage.SetPixel(scalingFactor * x + dx, scalingFactor * y + dy, c);
                        }
                    }
                }
            }
            canvas.Image = displayImage;
        }

        private void zoomSlider_ValueChanged(object sender, EventArgs e)
        {
            scalingFactor = 1 << zoomSlider.Value;
            zoomLabel.Text = $"{100 * scalingFactor}%";
            SetDisplayImage();
        }

        // TODO: actual load dialog, drag and drop support
        private void loadButton_Click(object sender, EventArgs e)
        {
            if (image != null)
            {
                image.Dispose();
            }
            if (brushes != null)
            {
                foreach (var b in brushes.Values)
                {
                    b.Dispose();
                }
            }

            image = new Bitmap(@"C:\Users\Matt\Desktop\stitch\shane.bmp");
            AnalyzeColors();
            SetDisplayImage();
        }
    }
}
