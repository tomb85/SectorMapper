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
        public double SectorFillThreshold { get; private set; }
        public int SectorIncrement { get; private set; }

        public SectorMapper(int sectorIncrement, double sectorFillThreshold)
        {
            SectorIncrement = sectorIncrement;
            SectorFillThreshold = sectorFillThreshold;
        }
        public SectorMap Map(Bitmap bitMapOriginal)
        {
            IDictionary<int, Sector> sectors = new SortedDictionary<int, Sector>();              //dictionary robi nastepujaca rzecz: trzyma sektory wg danego klucza; innymi slowy, dictionary to mapa obiektów 
            for (int x = 0; x < bitMapOriginal.Width; x++)
            {
                for (int y = 0; y < bitMapOriginal.Height; y++)
                {
                    int index = GetSectorIndex(x, y, bitMapOriginal.Width);     //dla danego pixela jaki jest ID sektora
                    if (!sectors.ContainsKey(index))                                             // czy dany sektor zostal juz dodany / czy juz posiada klucz; jeżeli nie, to:
                    {
                        sectors[index] = new Sector(id: index, fillTreshhold: SectorFillThreshold, width: SectorIncrement, height: SectorIncrement, globalX: x, globalY: y);
                        //Dictionary różni się od listy tym, że elementy w liście mają przypisany numer i kolejność, czyli [0] [1] itd. W dictionary kolejności nie ma, a do obiektów przypisujemy dowolną "nalepkę"
                        // tutaj używamy SortedDictionary, czyli mamy elementy ponumerowane; używamy tutaj IDictionary, bo potrzebujemy funkcjonalności dodania elementu w dowolnym miejscu, a nie na końcu (jak to jest w listach)
                        // a wynika to ze sposobu w jaki skanujemy bitmape - pixel po pixelu
                    }
                    var pixel = bitMapOriginal.GetPixel(x, y);
                    if (pixel.Name == "ff000000")               // pixel.Name to nazwa koloru w zapisie 16-stkowym, ff0000000 to kolor czarny
                    {
                        var sector = sectors[index];
                        sector.IncreaseFillCount();
                    }
                }
            }
            return new SectorMap(bitMapOriginal.Width, bitMapOriginal.Height, SectorIncrement, sectors.Select(GetSectorFromDictionaryEntry).ToList());
                                                                                              // to jest lista sektorów, to jest nasz żądany typ
                                                                                              // .Select to jest funkcja, której zadaniem jest zmapowanie obiektów jednego typu na obiekty innego typu
                                                                                              // .Select jako argumentu oczekuje funkcji (metody)
           

        }

        private int GetSectorIndex(int x, int y, int width)
        {
            int sectorsPerRow = width / SectorIncrement;
            return (x / SectorIncrement) + (y / SectorIncrement) * sectorsPerRow;
        }

        private Sector GetSectorFromDictionaryEntry(KeyValuePair<int, Sector> entry)
        {
            return entry.Value;
        }


            



    }
}
