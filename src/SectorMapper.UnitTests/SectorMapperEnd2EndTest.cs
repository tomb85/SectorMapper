using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace SectorMapper.UnitTests
{
    [TestClass]
    public class SectorMapperEnd2EndTest
    {
        [TestMethod]
        public void ShouldProduceDebugOutputWithSourceImage()
        {
            var mapper = new SectorMapper();
            var loader = new FixedSizeBitMapLoader(width: 500, height: 500);
            var bitMapOriginal = loader.LoadBitmap(path: "Data/input_img_0.bmp");
            var sectorMap = mapper.Map(bitMapOriginal);

            var creator = new CompositeBitmapCreatorBuilder().WithSource(original).Build();    //tu tworzymy obiekt "creator" który będzie użyty w SectorMapDebugger do tworzenia innych obiektów
            var sectorMapDebugger = new SectorMapDebugger(sectorMap, creator);                 // celem tej klasy jest dostep graficzny do efektu pracy
            sectorMapDebugger.Debug("/Results/source.bmp");
            Assert.IsTrue(File.Exists("/Results/source.bmp"));

        }
            
    }
}
