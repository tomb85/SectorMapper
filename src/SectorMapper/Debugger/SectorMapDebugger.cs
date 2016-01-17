using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SectorMapper.Debug
{
    public class SectorMapDebugger
    {
        private SectorMap sectorMap;
        private ICompositeBitmapCreator creator;

        public SectorMapDebugger(SectorMap sectorMap, ICompositeBitmapCreator creator)
        {            
            this.sectorMap = sectorMap;
            this.creator = creator;
        }

        public void Debug(string path)
        {
            var bitmap = creator.Create(sectorMap);
            bitmap.Save(path);
        }
    }
}
