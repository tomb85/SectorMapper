using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SectorMapper
{
    public class Sector
    {
        public int Id { get; private set; }                                         // { get; private set; } izoluje te wartosci od mozliwosci zmiany w innych częściach programu
        public double FillTreshhold { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Area { get { return Width * Height; } }
        public int FillCount { get; private set; }                                  
        public double FillRatio { get { return FillCount / (double)Area; } }        // Ponieważ Area i FillCount są typu unt, to kompilator zaklada że wynik też jest int, dlatego narzucamy typ wyniku
        public int GlobalX { get; private set; }
        public int GlobalY { get; private set; }
        private bool visited;
        private bool explored;

        public Sector(int id, double fillTreshhold, int width, int height, int globalX, int globalY)                // coort
        {
            Id = id;
            FillTreshhold = fillTreshhold;
            Width = width;
            Height = height;
            GlobalX = globalX;
            GlobalY = globalY;
        }

        public void IncreaseFillCount()
        {
            FillCount++;
        }

        public bool IsBlack()
        {
            return FillRatio > FillTreshhold;
        }

        public void MarkAsExplored()
        {
            explored = true;
        }

        public bool IsExplored()
        {
            return explored;
        }

        internal void MarkAsVisited()
        {
            visited = true;
        }

        internal bool IsVisited()
        {
            return visited;
        }
    }
}
