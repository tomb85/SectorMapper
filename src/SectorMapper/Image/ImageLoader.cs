using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace SectorMapper.Image
{
    public interface IImageLoader
    {
        Bitmap LoadBitmap(string path);
    }

    public class FixedSizeImageLoader : IImageLoader
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public FixedSizeImageLoader(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public Bitmap LoadBitmap(string path)
        {
            var bitmap = new Bitmap(path);
            if (bitmap.PixelFormat != PixelFormat.Format1bppIndexed)
            {
                throw new InvalidOperationException(String.Format("Unsuppoerted bitmap pixel format: {0}, must be: {1}", bitmap.PixelFormat, PixelFormat.Format1bppIndexed));
            }
            if (bitmap.Width != Width)
            {
                throw new InvalidOperationException(String.Format("Unsuppoerted bitmap width: {0}, must be: {1}", bitmap.Width, Width));
            }
            if (bitmap.Height != Height)
            {
                throw new InvalidOperationException(String.Format("Unsuppoerted bitmap height: {0}, must be: {2}", bitmap.Height, Height));
            }
            return bitmap;
        }
    }
}
