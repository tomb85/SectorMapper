using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SectorMapper
{
    public class SectorMapper
    {
        public int SectorIncrement { get; private set; }
        public double SectorFillThreshold { get; private set; }

        internal SectorMapper(SectorMapperBuilder builder)
        {
            SectorIncrement = builder.SectorIncrement;
            SectorFillThreshold = builder.SectorFillThreshold;
        }

        public SectorMap Map(Bitmap img)
        {
            IDictionary<int, Sector> sectors = new SortedDictionary<int, Sector>();         
            for (int x = 0; x < img.Width; x++)
            {
                for (int y = 0; y < img.Height; y++)
                {
                    int index = GetSectorIndex(x, y, img.Width, SectorIncrement);
                    if (!sectors.ContainsKey(index))
                    {
                        sectors[index] = new Sector(id: index, fillTreshhold: SectorFillThreshold, width: SectorIncrement, height: SectorIncrement);
                    }
                    var pixel = img.GetPixel(x, y);
                    if (pixel.Name == "ff000000")
                    {
                        var sector = sectors[index];
                        sector.IncreaseFillCount();
                    }     
                }
            }
            return new SectorMap(img.Width, img.Height, SectorIncrement, sectors.Select(kvp => kvp.Value).ToList());
        }

        private int GetSectorIndex(int x, int y, int imageWidth, int increment)
        {
            // Use row major
            int sectorsPerRow = imageWidth / increment;
            return (x / increment) + (y / increment) * sectorsPerRow;
        }
    }
}
