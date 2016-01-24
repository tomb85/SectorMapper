using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SectorMapper
{
    public class SectorCutInfoGenerator
    {
        private SectorMap sectorMap;
        private string path;
        private Stack<Sector> sectorStack = new Stack<Sector>();
        private List<String> outputBuffer = new List<String>();         //najpierw zbierzemy wszystkie nasze punkty do buffera, a potem je wyplujemy do pliku

        public SectorCutInfoGenerator(SectorMap sectorMap, string path)
        {
            this.sectorMap = sectorMap;
            this.path = path;
            
        }

        public void GenerateInfo(string bitmapName)
        {
            
            var startingSector = sectorMap.GetFirstSector();
            startingSector.MarkAsVisited();
            sectorStack.Push(startingSector);

            while (sectorStack.Count > 0)
            {
                var currentSector = sectorStack.Peek();
                Enter(currentSector, bitmapName);
                Resolve(currentSector);
            }
            using (var writer = new StreamWriter(path, true))
            {
                foreach (var line in outputBuffer)
                {
                    writer.WriteLine(line);
                }
            }
        }

        private void Resolve(Sector currentSector)
        {
            var nextSector = GetNextSector(currentSector);
            if (nextSector == null)
            {
                currentSector.MarkAsExplored();
                sectorStack.Pop();
            }
            else
            {
                nextSector.MarkAsVisited();
                sectorStack.Push(nextSector);
            }
        }

        private Sector GetNextSector(Sector currentSector)
        {
            var neighbours = GetNeighbours(currentSector);
            foreach (var sector in neighbours)
            {
                if (!sector.IsVisited() && !sector.IsExplored() && !sector.IsBlack()) 
                {
                    return sector;
                }
            }
            return null;
        }

        private List<Sector> GetNeighbours(Sector currentSector)
        {
            var neighbours = new List<Sector>();
            int rowNumber = currentSector.GlobalY / sectorMap.SectorIncrement;
            int colNumber = currentSector.GlobalX / sectorMap.SectorIncrement;
            int currentSectorID = currentSector.Id;
            int nextSectorID;

            //pierwszy sąsiad: rząd-1
            nextSectorID = currentSectorID - (sectorMap.Height / sectorMap.SectorIncrement);
            if (nextSectorID > 0)
            {
                neighbours.Add(sectorMap.GetSector(nextSectorID));
            }

            //drugi sąsiad: rząd+1
            nextSectorID = currentSectorID + (sectorMap.Height / sectorMap.SectorIncrement);
            if (nextSectorID < sectorMap.GetSectorCount())
            {
                neighbours.Add(sectorMap.GetSector(nextSectorID));
            }

            //trzeci sąsiad: kolumna+1
            nextSectorID = currentSectorID + 1;
            if (nextSectorID % (sectorMap.Width / sectorMap.SectorIncrement) != 0)
            {
                neighbours.Add(sectorMap.GetSector(nextSectorID));
            }

            //czwarty sąsiad: kolumna-1
            nextSectorID = currentSectorID - 1;
            if (currentSectorID % (sectorMap.Width / sectorMap.SectorIncrement) != 0)
            {
                neighbours.Add(sectorMap.GetSector(nextSectorID));
            }

            return neighbours;

        }

        private void Enter(Sector currentSector, string bitmapName)
        {
            int outputYvalue = currentSector.GlobalY / sectorMap.SectorIncrement;
            int outputXvalue = currentSector.GlobalX / sectorMap.SectorIncrement;
            var message = currentSector.Id + ": x = " + outputXvalue + ", y = " + outputYvalue + "  " + bitmapName.Split('.')[0];                          // new line: \n   ;   tab: \t
            outputBuffer.Add(message);

        }



    }
}
