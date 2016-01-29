using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Drawing;
using SectorMapper;

namespace SectorMapper.UnitTests
{
    [TestClass]
    public class SectorMapperEnd2EndTest
    {
        private SectorMapper mapper;
        private IBitMapLoader loader;
        private Bitmap bitMapOriginal;
        private SectorMap sectorMap;
        private SectorCutInfoGenerator sectorCutInfoGenerator;
        const string resultsPath = @"D:\inzgit\SectorMapper\wyniki";

        [TestInitialize]                            // ta metoda będzie wywoływana przed wywołaniem dowolnego testu w tej klasie (wynika to właśnie z test initialize)
        public void SetTestParameters()
        {
            mapper = new SectorMapper(25, 0.5);
            loader = new FixedSizeBitMapLoader(width: 500, height: 500);
            bitMapOriginal = loader.LoadBitmap(path: "Data/input_img_0.bmp");
            sectorMap = mapper.Map(bitMapOriginal);
            sectorCutInfoGenerator = new SectorCutInfoGenerator(sectorMap, resultsPath + Path.DirectorySeparatorChar + "TestOutput.txt");
            sectorCutInfoGenerator.GenerateInfo("01.bmp");
            if (!Directory.Exists(resultsPath))    // @ oznacza, że nie trzeba używać :\\ bo \ oznacza ścieżkę
            {
                Directory.CreateDirectory(resultsPath);
            }
            if (File.Exists(resultsPath + Path.DirectorySeparatorChar + "TestOutput.txt"))
            {
                File.Delete(resultsPath + Path.DirectorySeparatorChar + "TestOutput.txt");
            }

        }

        [TestMethod]
        [TestCategory("Acceptance")]
        public void ShouldProduceDebugOutputWithSourceImage()
        {                                                                                            // CompositeBitmapCreatorBuilder to nasze "menu" z którego wybieramy jakie operacje chcemy mieć przeprowadzone na załadowanej bitmapie. Istotna będzie kolejność, w jakiej to robimy.
            var creator = new CompositeBitmapCreatorBuilder().WithSource(bitMapOriginal).Build();    // tu tworzymy obiekt "creator" który będzie użyty w SectorMapDebugger do tworzenia innych obiektów            
            var sectorMapDebugger = new SectorMapDebugger(sectorMap, creator);                       // celem tej klasy jest dostep graficzny do efektu pracy
            sectorMapDebugger.Debug(resultsPath+Path.DirectorySeparatorChar+"source.bmp");
            Assert.IsTrue(File.Exists(resultsPath + Path.DirectorySeparatorChar + "source.bmp"));
            
        }   
        
        [TestMethod]
        [TestCategory("Acceptance")]

        public void ShouldProduceDebugOutputShowingSectorFill()
        {
            var creator = new CompositeBitmapCreatorBuilder().WithSectorFill(200).Build();
            var sectorMapDebugger = new SectorMapDebugger(sectorMap, creator);
            sectorMapDebugger.Debug(resultsPath + Path.DirectorySeparatorChar + "fillTest.bmp");
            Assert.IsTrue(File.Exists(resultsPath + Path.DirectorySeparatorChar + "fillTest.bmp"));
        }
                                                                            
            
        [TestMethod]
        [TestCategory("Acceptance")]
        public void ShouldProduceDebugOutputShowingSourceImageAndSectorFill()
        {
            var creator = new CompositeBitmapCreatorBuilder().WithSource(bitMapOriginal).WithSectorFill(201).Build();
            var sectorMapDebugger = new SectorMapDebugger(sectorMap, creator);
            sectorMapDebugger.Debug(resultsPath+Path.DirectorySeparatorChar+"originalANDsectorfill.bmp");
            Assert.IsTrue(File.Exists(resultsPath + Path.DirectorySeparatorChar + "originalANDsectorfill.bmp"));
        }

        [TestMethod]
        [TestCategory("Acceptance")]
        public void ShouldProduceSectorGrid()
        {
            var creator = new CompositeBitmapCreatorBuilder().WithSource(bitMapOriginal).WithSectorGrid().Build();
            var sectorMapDebugger = new SectorMapDebugger(sectorMap, creator);
            sectorMapDebugger.Debug(resultsPath + Path.DirectorySeparatorChar + "orgBMPwithSectorGrid.bmp");
            Assert.IsTrue(File.Exists(resultsPath + Path.DirectorySeparatorChar + "orgBMPwithSectorGrid.bmp"));
        }

        [TestMethod]
        [TestCategory("Acceptance")]
        public void ShouldProduceNumberedSectorGrid()
        {
            var creator = new CompositeBitmapCreatorBuilder().WithSource(bitMapOriginal).WithSectorGrid().WithSectorNumbering(10).Build();
            var sectorMapDebugger = new SectorMapDebugger(sectorMap, creator);
            sectorMapDebugger.Debug(resultsPath + Path.DirectorySeparatorChar + "org_grid_numeration.bmp");
            Assert.IsTrue(File.Exists(resultsPath + Path.DirectorySeparatorChar + "org_grid_numeration.bmp"));
        }
        [TestMethod]
        [TestCategory("Acceptance")]
        public void ShouldProduceSectoredNumberedWithGridLines()
        {
            var creator = new CompositeBitmapCreatorBuilder().WithSource(bitMapOriginal).WithSectorFill(122).WithSectorGrid().WithSectorNumbering(10).Build();
            var sectorMapDebugger = new SectorMapDebugger(sectorMap, creator);
            sectorMapDebugger.Debug(resultsPath + Path.DirectorySeparatorChar + "ShouldProduceSectoredNumberedWithGridLines.bmp");
            Assert.IsTrue(File.Exists(resultsPath + Path.DirectorySeparatorChar + "ShouldProduceSectoredNumberedWithGridLines.bmp"));
        }

        [TestMethod]
        [TestCategory("Acceptance")]
        public void ShouldProduceSectorsToBeCut()
        {
            var creator = new CompositeBitmapCreatorBuilder().WithSectorGrid().WithSectorsToBeCut(122).Build();
            var sectorMapDebugger = new SectorMapDebugger(sectorMap, creator);
            sectorMapDebugger.Debug(resultsPath + Path.DirectorySeparatorChar + "ShouldProduceSectorsToBeCut.bmp");
            Assert.IsTrue(File.Exists(resultsPath + Path.DirectorySeparatorChar + "ShouldProduceSectorsToBeCut.bmp"));
        }

        [TestMethod]
        [TestCategory("Acceptance")]
        public void ShouldProduceTXTfile()
        {
            
        }
    }
}

        