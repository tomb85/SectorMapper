using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SectorMapper.UnitTests
{
    [TestClass]
    public class SectorMapperBuilderTest
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void ShouldCreateSectorMapperWithCorrectParameters()
        {
            var mapper = new SectorMapperBuilder().WithSectorIncrement(10).WithSectorFillThreshold(0.33).Build();
            Assert.IsNotNull(mapper, "Mapper cannot be null");
            Assert.AreEqual(10, mapper.SectorIncrement, String.Format("Incorrect sector increment, expected {0} but got {1}", 10, mapper.SectorIncrement));
            Assert.AreEqual(0.33, mapper.SectorFillThreshold, String.Format("Incorrect sector fill threshold, expected {0} but got {1}", 0.33, mapper.SectorFillThreshold));
        }
    }
}
