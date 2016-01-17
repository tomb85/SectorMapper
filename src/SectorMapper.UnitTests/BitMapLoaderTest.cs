using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using System.Drawing.Imaging;


namespace SectorMapper.UnitTests
{
    [TestClass]
    public class BitMapLoaderTest
    {

        private const int CORRECT_HEIGHT = 500;
        private const int CORRECT_WIDTH = 500;

        [TestMethod]
        public void ShouldLoad1bppBitmapWithCorrectHeightAndWidth()
        {
            var loader = new FixedSizeBitMapLoader(CORRECT_WIDTH, CORRECT_HEIGHT);
            var loadedbitmap = loader.LoadBitmap("Data/input_img_0.bmp");

            Assert.AreEqual(CORRECT_HEIGHT, loadedbitmap.Height, "Height is not right");
            Assert.AreEqual(CORRECT_WIDTH, loadedbitmap.Width, "Width is not right");
            Assert.AreEqual(PixelFormat.Format1bppIndexed, loadedbitmap.PixelFormat, "Pixel format is " + loadedbitmap.PixelFormat + " and should be 1");



        }
    }
}
