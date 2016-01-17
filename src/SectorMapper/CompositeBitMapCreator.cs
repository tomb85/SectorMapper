using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SectorMapper
{
    interface ICompositeBitMapCreator
    {
        Bitmap CreateCompositeBitMap(SectorMap);
    }

    public abstract class AbstractCompositeBitmapCreator : ICompositeBitMapCreator 
    {
        private ICompositeBitMapCreator creator;

        public AbstractCompositeBitmapCreator(ICompositeBitMapCreator creator)
        {
            this.creator = creator;
        }
        
        public Bitmap CreateCompositeBitMap(SectorMap map)
        {
            var bitmap = CreateBitmap(map);
            if (creator != null)
            {
                var original = creator.Create(map);
                return original.Combine(bitmap);
            }
            else
            {
                return bitmap;
            }         
        }

        protected abstract Bitmap CreateBitmap(SectorMap map);
    }
}
