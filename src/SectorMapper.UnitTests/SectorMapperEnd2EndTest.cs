using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Drawing;

namespace SectorMapper.UnitTests
{
    [TestClass]
    public class SectorMapperEnd2EndTest
    {
        private SectorMapper mapper;
        private IBitMapLoader loader;
        private Bitmap bitMapOriginal;
        private SectorMap sectorMap;

        [TestInitialize]                            // ta metoda będzie wywoływana przed wywołaniem dowolnego testu w tej klasie (wynika to właśnie z test initialize)
        public void SetTestParameters()
        {
            mapper = new SectorMapper();
            loader = new FixedSizeBitMapLoader(width: 500, height: 500);
            bitMapOriginal = loader.LoadBitmap(path: "Data/input_img_0.bmp");
            sectorMap = mapper.Map(bitMapOriginal);
        }

        [TestMethod]
        [TestCategory("Acceptance")]
        public void ShouldProduceDebugOutputWithSourceImage()
        {                                                                                            // CompositeBitmapCreatorBuilder to nasze "menu" z którego wybieramy jakie operacje chcemy mieć przeprowadzone na załadowanej bitmapie. Istotna będzie kolejność, w jakiej to robimy.
            var creator = new CompositeBitmapCreatorBuilder().WithSource(bitMapOriginal).Build();    //tu tworzymy obiekt "creator" który będzie użyty w SectorMapDebugger do tworzenia innych obiektów            
            var sectorMapDebugger = new SectorMapDebugger(sectorMap, creator);                       // celem tej klasy jest dostep graficzny do efektu pracy
            sectorMapDebugger.Debug("source.bmp");
            Assert.IsTrue(File.Exists("source.bmp"));

        }   
        
        [TestMethod]
        [TestCategory("Acceptance")]

        public void ShouldProduceDebugOutputShowingSectorFill()
        {
            var creator = new CompositeBitmapCreatorBuilder().WithSectorFill().Build();
            var sectorMapDebugger = new SectorMapDebugger(sectorMap, creator);                 
            sectorMapDebugger.Debug("filltest.bmp");
            Assert.IsTrue(File.Exists("filltest.bmp"));
        }
                                                                            
            
        [TestMethod]
        [TestCategory("Acceptance")]
        public void ShouldProduceDebugOutputShowingSourceImageAndSectorFill()
        {
            var creator = new CompositeBitmapCreatorBuilder().WithSource(bitMapOriginal).WithSectorFill().Build();
            var sectorMapDebugger = new SectorMapDebugger(sectorMap, creator);
            sectorMapDebugger.Debug("originalANDsectorfill.bmp");
            Assert.IsTrue(File.Exists("originalANDsectorfill.bmp"));
        }
    }
}


//test ShouldProduceSectorGrid

//test ShouldProduceDebugOutputShowingSectorGridAndSourceImage