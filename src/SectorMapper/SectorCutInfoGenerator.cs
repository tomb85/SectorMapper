using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SectorMapper
{
    public class SectorCutInfoGenerator
    {
        private int tracerx;
        private int tracery;           //TO-DO zmienne do sledzenia ostatniego X oraz Y, do wyzerowania w [UUU]
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
            // [UUU]
            tracerx = 0;
            tracery = 0;
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
            
            var message = "LIN{X " + currentSector.GlobalX*0.4 + ",Y " + currentSector.GlobalY*0.4 + ",Z " + Convert.ToInt32(bitmapName.Split('.')[0])*5 + ",A 180,B 0,C 180}";                          // new line: \n   ;   tab: \t
            
            
            
            outputBuffer.Add(message); //TO-DO warunek na generacje linijki kodu - nie zawsze chcemy output

        }

        public static void AddHeader(string path)                  // static = inicjalizacja bez obiektu
        {
            using (var writer = new StreamWriter(path, true))
            {
                writer.WriteLine("&ACCESS RVP\r\n&REL 8\r\n&PARAM EDITMASK = *\r\n&PARAM TEMPLATE = C:\\KRC\\Roboter\\Template\\vorgabe\r\n&PARAM DISKPATH = KRC:\\R1\\Program\\frey\r\nDEF up01( )\r\n;FOLD INI;%{PE}\r\n  ;FOLD BASISTECH INI\r\n    GLOBAL INTERRUPT DECL 3 WHEN $STOPMESS==TRUE DO IR_STOPM ( )\r\n    INTERRUPT ON 3 \r\n    BAS (#INITMOV,0 )\r\n  ;ENDFOLD (BASISTECH INI)\r\n  ;FOLD USER INI\r\n    ;Make your modifications here\r\n\r\n  ;ENDFOLD (USER INI)\r\n;ENDFOLD (INI)\r\n\r\n;FOLD PTP HOME  Vel= 100 % DEFAULT;%{PE}%MKUKATPBASIS,%CMOVE,%VPTP,%P 1:PTP, 2:HOME, 3:, 5:100, 7:DEFAULT\r\n$BWDSTART = FALSE\r\nPDAT_ACT=PDEFAULT\r\nFDAT_ACT=FHOME\r\nBAS (#PTP_PARAMS,100 )\r\n$H_POS=XHOME\r\nPTP  XHOME\r\n;ENDFOLD\r\n;FOLD PTP P1 Vel=100 % PDAT1 Tool[2] Base[1]:inf;%{PE}%R 8.3.21,%MKUKATPBASIS,%CMOVE,%VPTP,%P 1:PTP, 2:P1, 3:, 5:100, 7:PDAT1\r\n$BWDSTART=FALSE\r\nPDAT_ACT=PPDAT1\r\nFDAT_ACT=FP1\r\nBAS(#PTP_PARAMS,100)\r\nPTP XP1 \r\n;ENDFOLD\r\n;FOLD LIN P2 CONT Vel=0.5 m/s CPDAT1 Tool[2] Base[1]:inf;%{PE}%R 8.3.21,%MKUKATPBASIS,%CMOVE,%VLIN,%P 1:LIN, 2:P2, 3:C_DIS C_DIS, 5:0.5, 7:CPDAT1\r\n$BWDSTART=FALSE\r\nLDAT_ACT=LCPDAT1\r\nFDAT_ACT=FP2\r\nBAS(#CP_PARAMS,0.5)\r\nLIN XP2 C_DIS C_DIS\r\n;ENDFOLD\r\n");         // \r\n to znak nowej linii
            }

        }

        public static void AddFooter(string path)
        {
            using (var writer = new StreamWriter(path, true))
            {
                writer.WriteLine("\r\n;FOLD PTP HOME  Vel= 100 % DEFAULT;%{PE}%MKUKATPBASIS,%CMOVE,%VPTP,%P 1:PTP, 2:HOME, 3:, 5:100, 7:DEFAULT\r\n$BWDSTART = FALSE\r\nPDAT_ACT=PDEFAULT\r\nFDAT_ACT=FHOME\r\nBAS (#PTP_PARAMS,100 )\r\n$H_POS=XHOME\r\nPTP  XHOME\r\n;ENDFOLD\r\n\r\nEND\r\n");
            }

        }


    }
}
