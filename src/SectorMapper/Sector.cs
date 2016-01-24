using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SectorMapper
{
    public class Sector
    {
        public int Id { get; private set; } //jest private, czyli jest to klasa tylko do odczytu, jedyna mozliwosc ustalenia wartosci = w konstrucktorze
        public double FillTreshhold { get; private set; } //odpowiednikiem w c++ byloby public getter + private setter + private property
        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Area { get { return Width * Height; } }
        public int FillCount { get; private set; } //uzywamy get private set aby "odizolowac" te wartosci od mozliwosci zmiany w innych czesciach programu
        public double FillRatio { get { return FillCount / (double)Area; } } // area i fill count to int, wiec kompilator autoamtycznie zaklada ze wynik operacji to tez jest int; czyli 11/50 = 0 a nie 0.22. Zeby wyswietlal wynik jako double, to co najmniej 1 argument musi byc double, i to robimy narzucajc typ wartosci (double)nazwazmiennej
        public int GlobalX { get; private set; }
        public int GlobalY { get; private set; }
        private bool visited;
        private bool explored;

        public Sector(int id, double fillTreshhold, int width, int height, int globalX, int globalY) //----*---- konstruktor
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
