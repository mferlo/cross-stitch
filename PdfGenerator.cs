
using System.Drawing;
using System.IO;

namespace Stitcher
{
    static class PdfGenerator
    {
        public static void Create(BinaryWriter bw, Bitmap image)
        {
            bw.Write("Hello World".ToCharArray());
        }
    }
}