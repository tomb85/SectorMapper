using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Drawing;

using SectorMapper.Image;
using SectorMapper.Debug;


namespace SectorMapper.UnitTests
{
    [TestClass]
    public class SectorMapperEndToEndTest
    {
        private const int ALPHA = 122;
        private SectorMapper mapper;
        private IImageLoader loader;
        private Bitmap original;
        private SectorMap sectorMap;

        [TestInitialize]
        public void Setup()
        {
            mapper = new SectorMapperBuilder().WithSectorIncrement(50).WithSectorFillThreshold(0.33).WithNumberingPolicy(NumberingPolicy.ZIG_ZAG).Build();
            loader = new FixedSizeImageLoader(500, 500);
            original = loader.LoadBitmap("Data/input_img_0.bmp");
            sectorMap = mapper.Map(original);

            if (!Directory.Exists("/Results"))
            {
                Directory.CreateDirectory("/Results");
            }  
        }

        [TestMethod]
        [TestCategory("Acceptance")]
        public void ShouldProduceDebugOutputWithSourceImage()
        {
            var creator = new CompositeBitmapCreatorBuilder().WithSource(original).Build();
            var sectorMapDebugger = new SectorMapDebugger(sectorMap, creator);
            sectorMapDebugger.Debug("/Results/source.bmp");                                
            Assert.IsTrue(File.Exists("/Results/source.bmp"));
        }

        [TestMethod]
        [TestCategory("Acceptance")]
        public void ShouldProduceDebugOutputShowingSectorFill()
        {
            var creator = new CompositeBitmapCreatorBuilder().WithFill(ALPHA).Build();
            var debugger = new SectorMapDebugger(sectorMap, creator);
            debugger.Debug("/Results/fill.bmp");
            Assert.IsTrue(File.Exists("/Results/fill.bmp"));
        }
        
        [TestMethod]
        [TestCategory("Acceptance")]
        public void ShouldProduceDebugOutputShowingSourceImageAndSectorFill()
        {
            var creator = new CompositeBitmapCreatorBuilder().WithSource(original).WithFill(ALPHA).Build();
            var debugger = new SectorMapDebugger(sectorMap, creator);
            debugger.Debug("/Results/source_fill.bmp");
            Assert.IsTrue(File.Exists("/Results/source_fill.bmp"));
        }

        [TestMethod]
        [TestCategory("Acceptance")]
        public void ShouldProduceDebugOutputShowingSectorGrid()
        {
            var creator = new CompositeBitmapCreatorBuilder().WithGrid().Build();
            var debugger = new SectorMapDebugger(sectorMap, creator);
            debugger.Debug("/Results/grid.bmp");
            Assert.IsTrue(File.Exists("/Results/grid.bmp"));
        }

        [TestMethod]
        [TestCategory("Acceptance")]
        public void ShouldProduceDebugOutputShowingSectorGridAndSourceImage()
        {
            var creator = new CompositeBitmapCreatorBuilder().WithSource(original).WithGrid().Build();
            var debugger = new SectorMapDebugger(sectorMap, creator);
            debugger.Debug("/Results/grid_source.bmp");
            Assert.IsTrue(File.Exists("/Results/grid_source.bmp"));
        }

        [TestMethod]
        [TestCategory("Acceptance")]
        public void ShouldProduceDebugOutputShowingSectorGridAndSectorFill()
        {
            var creator = new CompositeBitmapCreatorBuilder().WithFill(ALPHA).WithGrid().Build();
            var debugger = new SectorMapDebugger(sectorMap, creator);
            debugger.Debug("/Results/grid_fill.bmp");
            Assert.IsTrue(File.Exists("/Results/grid_fill.bmp"));
        }

        [TestMethod]
        [TestCategory("Acceptance")]
        public void ShouldProduceDebugOutputShowingAllFilters()
        {
            var creator = new CompositeBitmapCreatorBuilder().WithSource(original).WithFill(alpha: 122).WithGrid().WithSectorNumbers(fontSize: 12).Build();
            var debugger = new SectorMapDebugger(sectorMap, creator);
            debugger.Debug("/Results/all.bmp");
            Assert.IsTrue(File.Exists("/Results/all.bmp"));
        }

        [TestMethod]
        [TestCategory("Acceptance")]
        public void ShouldProduceDebugOutputShowingSectorNumbers()
        {
            var creator = new CompositeBitmapCreatorBuilder().WithSectorNumbers(8).Build();
            var debugger = new SectorMapDebugger(sectorMap, creator);
            debugger.Debug("/Results/numbers.bmp");
            Assert.IsTrue(File.Exists("/Results/numbers.bmp"));
        }
    }
}
