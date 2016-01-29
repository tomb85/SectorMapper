using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace SectorMapper.Tool
{
    class Program
    {
        private SectorMapper mapper;
        private IBitMapLoader loader;
        private const string SECTOR_INCREMENT = "secInc";
        private const string FILL_TRESHHOLD= "fillT";
        private const string BITMAP_WIDTH = "w";
        private const string BITMAP_HEIGHT = "h";
        private const string SOURCE_DIRECTORY = "in";
        private const string OUTPUT = "out";
        private Dictionary<string, string> parameters;
        
        static void Main(string[] args)
        {            
            var program = new Program();
            program.Run(args);
        }

        /* 
         * argumenty które potrzebujemy do wywołania funkcji to:
         * - sectorIncrement [secInc] [int] [true]
         * - fillTreshhold [fillT] [double] [false] [0.5]
         * - Bitmap width [w] [int] [true]
         * - Bitmap height [h] [int] [true]
         * - source directory [in] [string] [true]
         * - output [out] [string] [false] [up1.txt]
         * 
         * mapper.exe --secInc=50 --in="c:\\test.bmp" --out="c:\\up1.txt"
         */

        private string GetKey(string arg)
        {
            return arg.Split('=')[0].Replace("--", "");
        }

        private string GetValue(string arg)
        {
            return arg.Split('=')[1];
        }

        private void ParseArguments(string[] args)
        {
            parameters = args.ToDictionary(GetKey, GetValue);

            if (!parameters.ContainsKey(SECTOR_INCREMENT))
            {
                throw new InvalidOperationException("wymagane SECTOR_INCREMENT");
            }

            if (!parameters.ContainsKey(BITMAP_WIDTH))
            {
                throw new InvalidOperationException("wymagane BITMAP_WIDTH");
            }

            if (!parameters.ContainsKey(BITMAP_HEIGHT))
            {
                throw new InvalidOperationException("wymagane BITMAP_HEIGHT");
            }

            if (!parameters.ContainsKey(SOURCE_DIRECTORY))
            {
                throw new InvalidOperationException("wymagane SOURCE_DIRECTORY");
            }

            if (!parameters.ContainsKey(FILL_TRESHHOLD))
            {
                parameters[FILL_TRESHHOLD] = "0.5";
            }

            if (!parameters.ContainsKey(OUTPUT))
            {
                parameters[OUTPUT] = "up1.src";
            }

        }

        private void Run(string[] args)                     
        {
            ParseArguments(args);

            if (File.Exists(parameters[OUTPUT]))
            {
                File.Delete(parameters[OUTPUT]);
            }
            int sectorIncrement = Int32.Parse(parameters[SECTOR_INCREMENT]);
            double fillTreshhold = Double.Parse(parameters[FILL_TRESHHOLD]);
            int bmpWidth = Int32.Parse(parameters[BITMAP_WIDTH]);
            int bmpHeight = Int32.Parse(parameters[BITMAP_HEIGHT]);

            mapper = new SectorMapper(sectorIncrement, fillTreshhold);
            loader = new FixedSizeBitMapLoader(bmpWidth, bmpHeight);

            SectorCutInfoGenerator.AddHeader(parameters[OUTPUT]);

            foreach (var bmpfile in Directory.GetFiles(parameters[SOURCE_DIRECTORY], "*.bmp"))
            {
                var bmp = loader.LoadBitmap(bmpfile);
                var map = mapper.Map(bmp);
                var writer = new SectorCutInfoGenerator(map, parameters[OUTPUT]);
                writer.GenerateInfo(Path.GetFileName(bmpfile));
            }

            SectorCutInfoGenerator.AddFooter(parameters[OUTPUT]);
        }
    }
}
