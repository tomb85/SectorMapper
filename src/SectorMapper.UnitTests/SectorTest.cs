using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

namespace SectorMapper.UnitTests
{
    [TestClass]
    public class SectorTest
    {
        [TestMethod]
        public void TestMethod1()
        {
            var mockSector = new Mock<Sector>();
            Assert.IsNotNull(mockSector);

            // TODO
        }
    }
}
