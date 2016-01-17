using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;


namespace SectorMapper
{
    public interface IBitMapLoader
    {
        Bitmap LoadBitmap(string path);
    }

    public class FixedSizeBitMapLoader : IBitMapLoader
    {
        public int ExpectedWidth {get; private set;}
        public int ExpectedHeight { get; private set; }

        public FixedSizeBitMapLoader(int width, int height)
        {
            ExpectedWidth = width;
            ExpectedHeight = height;
        }


        public Bitmap LoadBitmap(string path)
        {
            var loadedbitmap = new Bitmap(path);
            if (loadedbitmap.PixelFormat != PixelFormat.Format1bppIndexed)
            {
                throw new InvalidOperationException("Expected image w/ 1bpp, got image w/ " + PixelFormat.Format1bppIndexed + " bpp");  
            }

            if (loadedbitmap.Width != ExpectedWidth)
            {
                throw new InvalidOperationException("Expected image width should be "+ ExpectedWidth + " got image with" + loadedbitmap.Width);  
            }

            if (loadedbitmap.Height != ExpectedHeight)
            {
                throw new InvalidOperationException("Expected image Height should be " + ExpectedHeight + " got image with" + loadedbitmap.Height);
            }

            return loadedbitmap;
        }


    }

}
