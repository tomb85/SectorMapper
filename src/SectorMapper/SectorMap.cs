using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SectorMapper
{
    public class SectorMap
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int SectorIncrement { get; private set; }
        public List<Sector> SectorList { get; private set; } 

        public SectorMap(int width, int height, int sectorIncrement, List<Sector> sectorList)
        {
            Width = width;
            Height = height;
            SectorIncrement = sectorIncrement;
            SectorList = sectorList;            
        }

        public Sector GetFirstSector()
        {
            return SectorList[0];
        }

        public bool IsSectorFilled(int x, int y)         
        {
            var sector = SectorList[GetSectorIndex(x, y)];       
            return sector.IsBlack();
        }

        private int GetSectorIndex(int x, int y)                // to jest metoda mapująca / numerująca = czyli na podstawie wsp. x, y wysmaży ID (numer) sektora.
        {
            int sectorsPerRow = Width / SectorIncrement;
            return (x / SectorIncrement) + (y / SectorIncrement) * sectorsPerRow;
                    //poniewaz x oraz SectorIncrement są typu int, to wynik dzielenia int/int da też int, czyli da numer kolumny (nie martwimy się o wartości po przecinku, bo to nie ten typ zmiennej)
        }

        public bool IsItGridPixel(int x, int y)
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

        public bool IsSectorToBeCut(int x, int y)
        {
            var sector = SectorList[GetSectorIndex(x, y)];       
            return sector.IsExplored();
        }

        public Sector GetSector(int nextSectorID)
        {
            return SectorList[nextSectorID];
        }

        public int GetSectorCount()
        {
            return SectorList.Count;
        }
    }
}
