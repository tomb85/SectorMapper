using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SectorMapper
{
    public class SectorMapperBuilder
    {
        public int SectorIncrement { get; private set; }
        public double SectorFillThreshold { get; private set; }
        public NumberingPolicy NumberingPolicy { get; private set; }

        public SectorMapperBuilder WithSectorIncrement(int sectorIncrement)
        {
            SectorIncrement = sectorIncrement;
            return this;
        }

        public SectorMapperBuilder WithSectorFillThreshold(double sectorFillThreshold)
        {
            SectorFillThreshold = sectorFillThreshold;
            return this;
        }

        public SectorMapperBuilder WithNumberingPolicy(NumberingPolicy numberingPolicy)
        {
            NumberingPolicy = numberingPolicy;
            return this;
        }

        public SectorMapper Build()
        {
            return new SectorMapper(this);
        }
    }
}
