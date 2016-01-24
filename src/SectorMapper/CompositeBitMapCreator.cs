using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SectorMapper
{
    public interface ICompositeBitMapCreator
    {
        Bitmap CreateCompositeBitMap(SectorMap map);
    }

    public abstract class AbstractCompositeBitmapCreator : ICompositeBitMapCreator              //używamy abstract class ponieważ operujemy na algorytmie, 
    {
        private ICompositeBitMapCreator creator;

        public AbstractCompositeBitmapCreator(ICompositeBitMapCreator creator)          //---*--- konstruktor
        {
            this.creator = creator;
        }
        
        public Bitmap CreateCompositeBitMap(SectorMap map)
        {
            var bitmap = CreateBitmap(map);
            if (creator != null)
            {
                var original = creator.CreateCompositeBitMap(map);
                return original.Combine(bitmap);                                // metoda Combine ma  połączyć 2+ bitmapy w jeden obraz
            }
            else
            {
                return bitmap;
            }         
        }

        protected abstract Bitmap CreateBitmap(SectorMap map);
    }

    class AddSectorNumeration : AbstractCompositeBitmapCreator
    {
        private int fontSize; 
        public AddSectorNumeration(ICompositeBitMapCreator creator, int fontSize) : base(creator)
        {
            this.fontSize = fontSize;
        }

        protected override Bitmap CreateBitmap(SectorMap map)
        {
            var bitmap = new Bitmap(map.Width, map.Height);             // tworzymy bitmape, ktora bedziemy modyfikowac
            using (var graphics = Graphics.FromImage(bitmap))           // tworzymy Graphics.Object z biblioteki .NET; ten typ obiektu pozwala na bardziej zaawansowane operacje na bitmapie - pisanie, kolorowanie itp. szczegóły są w dokumentacji metody
           // using to skrót do compiler'a, aby kompiler wiedział, że operuje na system resources - czyli za użytkownika alokuje i zwalnia zasoby systemowe, uzytkownik sie tym nie musi martwic
            {
                var stringFormat = new StringFormat();                  // to definiuje jakie maja być właściwości wyświetlanego tekstu
                stringFormat.Alignment = StringAlignment.Center;        
                stringFormat.LineAlignment = StringAlignment.Center;

                var font = new Font("Courier New", fontSize);

                foreach (var sector in map.SectorList)                     // foreach = iteruje wszystkie sektory
                {
                    var rect = new Rectangle(sector.GlobalX, sector.GlobalY, map.SectorIncrement, map.SectorIncrement);     // dla danego sektora tworzymy prostokąt (rectangle) ktory mowi, jaka jest X i Y wzgledem calej bitmapy oraz jaka jest dlugosc i szerokosc tego prostokąta
                    graphics.DrawString(sector.Id.ToString(), font, Brushes.Black, rect, stringFormat);     //metoda graphics.DrawString to po prostu maluje; rect = gdzie; stringFormat = jaki format
                }

                graphics.Flush();                                       // wszystkie operacje powyzej sa trzymane w buforze. Dopoki nie wywolasz flush, to ten obiekt nie bedzie zupdate'owany
            }
            return bitmap;
        }
    }


    class MarkSectorsToBeCut : AbstractCompositeBitmapCreator
    {
        private int alfa;
        public MarkSectorsToBeCut(ICompositeBitMapCreator creator, int alfa) : base(creator)
        {
            this.alfa = alfa;
        }

        protected override Bitmap CreateBitmap(SectorMap map)
        {
            var outputBitmap = new Bitmap(map.Width, map.Height);
            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    if (map.IsSectorToBeCut(x, y))
                    {
                        outputBitmap.SetPixel(x, y, Color.FromArgb(alfa, Color.Green));
                    }
                    else
                    {
                        outputBitmap.SetPixel(x, y, Color.FromArgb(0, Color.White));
                    }
                }
            }
            return outputBitmap;
        }

    }

    class AddSectorGrid : AbstractCompositeBitmapCreator
    {
        public AddSectorGrid(ICompositeBitMapCreator creator) : base(creator)
        {
            //no-op
        }

        protected override Bitmap CreateBitmap(SectorMap map)
        {

            var outputBitmap = new Bitmap(map.Width, map.Height);
            for (int x = 1; x < map.Width; x++)
            {
                for (int y = 1; y < map.Height; y++)
                {
                    if (map.IsItGridPixel(x,y))
                    {
                        outputBitmap.SetPixel(x, y, Color.Blue);
                    }
                    else
                    {
                        outputBitmap.SetPixel(x, y, Color.FromArgb(0, Color.White));
                    }
                }
            }
            return outputBitmap;
        }
    }

    class AddSectorFill : AbstractCompositeBitmapCreator
    {
        private int alfa;
        public AddSectorFill(ICompositeBitMapCreator creator, int alfa=122) : base(creator)
        {
            this.alfa = alfa;
        }

        protected override Bitmap CreateBitmap(SectorMap map)
        {
            var outputBitmap = new Bitmap(map.Width, map.Height);
            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    if (map.IsSectorFilled(x, y))
                    {
                        outputBitmap.SetPixel(x, y, Color.FromArgb(alfa,Color.Red));
                    }
                    else
                    {
                        outputBitmap.SetPixel(x, y, Color.FromArgb(0,Color.White));
                    }
                }
            }
            return outputBitmap;
        }
    }


    class AddSourceBitMap : AbstractCompositeBitmapCreator
    {
        private Bitmap source;
        public AddSourceBitMap(ICompositeBitMapCreator creator, Bitmap source) : base(creator)           //"base" to keyword wywołujący klasę bazową (w tym przypadku konstruktor) base można używać na poziomie konstruktora albo na poziomie metody.
        {
            this.source = source;       // czyli konkretnie przypisuje source do "private Bitmap source;" z tego, który jest w konstruktorze
        }


        protected override Bitmap CreateBitmap(SectorMap map)       // ponieważ ta metoda jest zadeklarowana w klasie abstrakcyjnej, to należy poinstruować kompiler, że jest to implementacja tej metody.
        {
            return source;
        }
    }

    public class CompositeBitmapCreatorBuilder                      // celem tej klasy jest stworzenie mozliwosci nieskończonego dodawania elementów do łańcucha wywołania
    {                                                               // w var creator = new CompositeBitmapCreatorBuilder().WithSource(bitMapOriginal).Build(); możemy dać .WithSource(x).WithSource(y).WithSource(z).Build();
        private ICompositeBitMapCreator creator;
        public ICompositeBitMapCreator Build()
        {
            return creator;
        }

        public CompositeBitmapCreatorBuilder WithSource(Bitmap source)
        {
            creator = new AddSourceBitMap (creator, source);
            return this;
        }

        public CompositeBitmapCreatorBuilder WithSectorFill(int alfa)
        {
            creator = new AddSectorFill(creator, alfa);
            return this;
        }

        public CompositeBitmapCreatorBuilder WithSectorGrid()
        {
            creator = new AddSectorGrid(creator);
            return this;
        }

        public CompositeBitmapCreatorBuilder WithSectorNumbering(int fontSize)
        {
            creator = new AddSectorNumeration(creator, fontSize);
            return this;
        }

        public CompositeBitmapCreatorBuilder WithSectorsToBeCut(int alfa)
        {
            creator = new MarkSectorsToBeCut(creator, alfa);
            return this;
        }
    }



}














