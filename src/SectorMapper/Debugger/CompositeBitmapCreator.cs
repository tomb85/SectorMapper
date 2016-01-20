using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.Drawing.Drawing2D;

namespace SectorMapper.Debug
{
    public interface ICompositeBitmapCreator
    {
        Bitmap Create(SectorMap map);
    }
    
    public abstract class AbstractCompositeBitmapCreator : ICompositeBitmapCreator 
    {
        private ICompositeBitmapCreator creator;

        public AbstractCompositeBitmapCreator(ICompositeBitmapCreator creator)
        {
            this.creator = creator;
        }
        
        public Bitmap Create(SectorMap map)
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


    internal class AddGrid : AbstractCompositeBitmapCreator
    {
        public AddGrid(ICompositeBitmapCreator creator)
            : base(creator)
        {
            // no-op
        }

        protected override Bitmap CreateBitmap(SectorMap map)
        {
            var bitmap = new Bitmap(map.Width, map.Height);
            for (int x = 1; x < map.Width; x++)
            {
                for (int y = 1; y < map.Height; y++)
                {
                    if (map.IsGrid(x, y))
                    {
                        bitmap.SetPixel(x, y, Color.Blue);
                    }
                    else
                    {
                        bitmap.SetPixel(x, y, Color.FromArgb(0, Color.White));
                    }
                }
            }
            return bitmap;
        }
    }

    internal class AddSectorNumbers : AbstractCompositeBitmapCreator
    {
        private int fontSize;

        public AddSectorNumbers(ICompositeBitmapCreator creator, int fontSize) : base(creator)
        {
            this.fontSize = fontSize;
        }

        protected override Bitmap CreateBitmap(SectorMap map)
        {
            var bitmap = new Bitmap(map.Width, map.Height);           
            using (var graphics = Graphics.FromImage(bitmap))
            {
                var stringFormat = new StringFormat();
                stringFormat.Alignment = StringAlignment.Center;
                stringFormat.LineAlignment = StringAlignment.Center;

                var font = new Font("Courier New", fontSize);
                               
                foreach(var sector in map.Sectors) {
                    var rect = new Rectangle(sector.GlobalX, sector.GlobalY, map.SectorIncrement, map.SectorIncrement);
                    graphics.DrawString(sector.Id.ToString(), font, Brushes.Black, rect, stringFormat);               
                }
                                
                graphics.Flush();
            }
            return bitmap;          
        }
    }

    internal class AddFill : AbstractCompositeBitmapCreator
    {
        private int alpha;

        public AddFill(ICompositeBitmapCreator creator, int alpha)
            : base(creator)
        {
            this.alpha = alpha;
        }
        
        protected override Bitmap CreateBitmap(SectorMap map)
        {
            var bitmap = new Bitmap(map.Width, map.Height);
            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {                   
                    if (map.IsFill(x, y))
                    {
                        bitmap.SetPixel(x, y, Color.FromArgb(alpha, Color.Red));
                    }
                    else
                    {
                        bitmap.SetPixel(x, y, Color.FromArgb(0, Color.White));
                    }
                }
            }            
            return bitmap;               
        }
    }

    internal class AddSource : AbstractCompositeBitmapCreator
    {
        private Bitmap source;

        public AddSource(ICompositeBitmapCreator creator, Bitmap source) : base(creator)
        {
            this.source = source;
        }

        protected override Bitmap CreateBitmap(SectorMap map)
        {
            return source;
        }
    }

    public class CompositeBitmapCreatorBuilder
    {
        private ICompositeBitmapCreator creator;

        public CompositeBitmapCreatorBuilder WithGrid()
        {
            creator = new AddGrid(creator);
            return this;
        }

        public CompositeBitmapCreatorBuilder WithFill(int alpha)
        {
            creator = new AddFill(creator, alpha);
            return this;
        }

        public CompositeBitmapCreatorBuilder WithSource(Bitmap source)
        {
            creator = new AddSource(creator, source);
            return this;
        }

        public CompositeBitmapCreatorBuilder WithSectorNumbers(int fontSize)
        {
            creator = new AddSectorNumbers(creator, fontSize);
            return this;
        }

        public ICompositeBitmapCreator Build()
        {
            return creator;
        }
    }
}