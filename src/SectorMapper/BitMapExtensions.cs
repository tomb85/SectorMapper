using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SectorMapper
{
    static class BitMapExtensions           // klasa static pozwala na inicjalizację klasy bez potrzeby tworzenia obiektu
    {
        public static Bitmap Combine(this Bitmap background, Bitmap source)            // mamy "this" ponieważ this reporezentuje obiekt, który tą metodę wywołuje
        {
            var combined = new Bitmap(background.Width, background.Height);
            for (int x = 0; x < background.Width; x++)
            {
                for (int y = 0; y < background.Height; y++)
                {
                    // Use Alpha Blending
                    // displayColor = sourceColor × alpha / 255 + backgroundColor × (255 – alpha) / 255

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
