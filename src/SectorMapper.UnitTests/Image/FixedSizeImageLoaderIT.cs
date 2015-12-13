using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing.Imaging;
using System.Drawing;

using SectorMapper.Image;

namespace SectorMapper.UnitTests.Image
{
    [TestClass]
    public class FixedSizeImageLoaderIT
    {
        private const int SIZE_500 = 500;
          
        [TestMethod]
        [TestCategory("Integrtion")]        
        public void ShouldLoadMonochromeBitmapWithCorrectSize()
        {            
            var loader = new FixedSizeImageLoader(SIZE_500, SIZE_500);
            var bitmap = loader.LoadBitmap("Data/input_img_0.bmp");              
            Assert.AreEqual(bitmap.PixelFormat, PixelFormat.Format1bppIndexed, String.Format("Pixel format is {0} and should be {1}", bitmap.PixelFormat, PixelFormat.Format1bppIndexed));
            Assert.AreEqual(SIZE_500, bitmap.Width, String.Format("Width is {0} and should be {1}", bitmap.Width, SIZE_500));
            Assert.AreEqual(SIZE_500, bitmap.Height, String.Format("Height is {0} and should be {1}", bitmap.Height, SIZE_500));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        [TestCategory("Integrtion")]
        public void ShoulThrowIOEIfIncorrectPixelFormat()
        {
            var loader = new FixedSizeImageLoader(SIZE_500, SIZE_500);
            loader.LoadBitmap("Data/input_img_1.bmp"); 
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        [TestCategory("Integrtion")]
        public void ShoulThrowIOEIfWrongSize()
        {
            var loader = new FixedSizeImageLoader(SIZE_500, SIZE_500);
            loader.LoadBitmap("Data/input_img_2.bmp");
        }
    }
}
