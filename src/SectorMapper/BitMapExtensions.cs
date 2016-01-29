using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SectorMapper
{
    static class BitMapExtensions                                                       // static = inicjalizacja bez obiektu
    {
        public static Bitmap Combine(this Bitmap background, Bitmap source)             // tutaj "this" to obiekt jednocześnie reprezentujący klasę
        {
            var combined = new Bitmap(background.Width, background.Height);
            for (int x = 0; x < background.Width; x++)
            {
                for (int y = 0; y < background.Height; y++)
                {
                    // używamy tzw. Alpha Blending

                    var sourcePixel = source.GetPixel(x, y);
                    var backgroundPixel = background.GetPixel(x, y);
                    int alpha = sourcePixel.A;

                    int red = sourcePixel.R * alpha / 255 + backgroundPixel.R  * (255 - alpha) / 255;
                    int green = sourcePixel.G  * alpha / 255 + backgroundPixel.G  * (255 - alpha) / 255;
                    int blue = sourcePixel.B *  alpha / 255 + backgroundPixel.B * (255 - alpha) / 255;

                    combined.SetPixel(x, y, Color.FromArgb(red, green, blue));                   
                }
            }
            return combined;
  
        }
    }
}
