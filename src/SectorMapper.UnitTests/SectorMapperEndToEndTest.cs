﻿using System;
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
        private SectorMapper mapper;
        private IImageLoader loader;
        private Bitmap original;
        private SectorMap sectorMap;

        [TestInitialize]
        public void Setup()
        {
            mapper = new SectorMapperBuilder().WithSectorIncrement(100).WithSectorFillThreshold(0.33).Build();
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
            var creator = new CompositeBitmapCreatorBuilder().WithFill(122).Build();
            var debugger = new SectorMapDebugger(sectorMap, creator);
            debugger.Debug("/Results/fill.bmp");
            Assert.IsTrue(File.Exists("/Results/fill.bmp"));
        }
        
        [TestMethod]
        [TestCategory("Acceptance")]
        public void ShouldProduceDebugOutputShowingSourceImageAndSectorFill()
        {
            var creator = new CompositeBitmapCreatorBuilder().WithSource(original).WithFill(122).Build();
            var debugger = new SectorMapDebugger(sectorMap, creator);
            debugger.Debug("/Results/combined.bmp");
            Assert.IsTrue(File.Exists("/Results/combined.bmp"));
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
        public void ShouldProduceDebugOutputShowingAllFilters()
        {
            var creator = new CompositeBitmapCreatorBuilder().WithSource(original).WithFill(122).WithGrid().Build();
            var debugger = new SectorMapDebugger(sectorMap, creator);
            debugger.Debug("/Results/all.bmp");
            Assert.IsTrue(File.Exists("/Results/all.bmp"));
        }
    }
}
