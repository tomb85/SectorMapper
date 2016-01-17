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
            return new SectorMap();
        }
    }
}
