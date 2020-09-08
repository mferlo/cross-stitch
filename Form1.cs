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
        List<Color> colors;

        public Form()
        {
            InitializeComponent();
            scalingFactor = 1;
            loadButton_Click(null, null);
        }

        /*
         TODO:
            sort colors
            display colors better (RGB + color swatch)
            highlight colors on selection
         */

        private void AnalyzeColors()
        {
            var cs = new HashSet<Color>();
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    cs.Add(image.GetPixel(x, y));
                }
            }
            colors = cs.ToList();
            colorListBox.Items.AddRange(colors.Select(c => c.ToString()).ToArray<object>());
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
            image = new Bitmap(@"C:\Users\Matt\Desktop\stitch\shane.bmp");

            AnalyzeColors();
            SetDisplayImage();
        }
    }
}
