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

    class AddSectorGrid : AbstractCompositeBitmapCreator
    {
        public AddSectorGrid(ICompositeBitMapCreator creator) : base(creator)
        {
            //no-op
        }

        protected override Bitmap CreateBitmap(SectorMap map)
        {
            //for / if do wygenerowania x y wsp mapy
        }
    }

    class AddSectorFill : AbstractCompositeBitmapCreator
    {
        public AddSectorFill(ICompositeBitMapCreator creator) : base(creator)
        {
            // no-op
            // no-op oznacza, że to pole jest puste intencjonalnie i nic nie należy tu wpisywać
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
                        outputBitmap.SetPixel(x, y, Color.Red);
                    }
                    else
                    {
                        outputBitmap.SetPixel(x, y, Color.White);
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

        public CompositeBitmapCreatorBuilder WithSectorFill()
        {
            creator = new AddSectorFill(creator);
            return this;
        }

        public CompositeBitmapCreatorBuilder WithSectorGrid()
        {
            creator = new AddSectorGrid(creator);
            return this;
        }
    }

}














