using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SectorMapper
{
    public class SectorMap
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int SectorIncrement { get; private set; }
        public IList<Sector> Sectors { get; private set; }
        public NumberingPolicy NumberingPolicy { get; private set; }

        public SectorMap(int width, int height, int sectorIncrement, IList<Sector> sectors, NumberingPolicy numberingPolicy = NumberingPolicy.LINEAR)
        { 
            Width = width;
            Height = height;
            SectorIncrement = sectorIncrement;
            Sectors = sectors;
            NumberingPolicy = numberingPolicy;

            if (!ValidateSectorIncrement())
            {
                throw new InvalidOperationException("Incorrect sector increment specified");
            }

            if (!ValidateSectorCount())
            {
                throw new InvalidOperationException(String.Format("Incorrect sector count, expected {0} but got {1}", GetCorrectSectorCount(), sectors.Count));
            }          
        }

        private bool ValidateSectorIncrement()
        {
            return (Width * Height) % (SectorIncrement * SectorIncrement) == 0;
        }

        private bool ValidateSectorCount()
        {
            return (Width * Height) / (SectorIncrement * SectorIncrement) == Sectors.Count;
        }

        private int GetCorrectSectorCount()
        {
            return (Width * Height) / (SectorIncrement * SectorIncrement);
        }

        private int GetSectorIndex(int x, int y)
        {
            if (NumberingPolicy == NumberingPolicy.ZIG_ZAG)
            {
                // transform x for odd rows
                int rowNumber = y / SectorIncrement;
                if (rowNumber % 2 != 0)
                {
                    x = Width - x - 1;
                }
            }
            // Use row major
            int sectorsPerRow = Width / SectorIncrement;
            return (x / SectorIncrement) + (y / SectorIncrement) * sectorsPerRow;
        }

        public bool IsFill(int x, int y)
        {
            var sector = Sectors[GetSectorIndex(x, y)];
            return sector.IsBlack();            
        }

        public bool IsGrid(int x, int y)
        {
            if (x % SectorIncrement == 0 || y % SectorIncrement == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }                
    }
}
