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

        public SectorMap()
        {
            Width = 500;
            Height = 500;
        }

        public bool IsSectorFilled(int x, int y)          // TO JEST TYLKO TYMCZASOWA IMPLEMENTACJA = DO ZMIANY!!!!
        {
            if (x < 200)
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
