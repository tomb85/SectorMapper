using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace SectorMapper.UnitTests
{
    [TestClass]
    public class SectorMapTest
    {
        [TestMethod]
        [TestCategory("Unit")]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ConstructorShouldThrowIOEWhenArgumentsAreInvalid()
        {
            var map = new SectorMap(500, 500, 33, new List<Sector>());
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void ShouldConstructForValidArguments()
        {
            var sectors = Enumerable.Repeat(new Sector(1, 0.1, 10, 10), 100).ToList();
            var map = new SectorMap(500, 500, 50, sectors);
        }
    }
}
