using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SectorMapper
{
    public class SectorMapDebugger
    {
        private SectorMap map;
        private ICompositeBitMapCreator creator;
        public SectorMapDebugger(SectorMap map, ICompositeBitMapCreator creator)
        {
            this.map = map;
            this.creator = creator;
        }

        public void Debug(string path)
        {
            var output = creator.CreateCompositeBitMap(map);
            output.Save(path);
        }
    }
}
