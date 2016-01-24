using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace SectorMapper.UnitTests
{
    [TestClass]
    public class SectorTest
    {
        [TestMethod]
        [TestCategory("Unit")]
        public void ConstructorShouldReturnGoodArguments()
        {
            var testowysector = new Sector(12,10,4,6,0,0); //var gwarantuje, ze stworzony obiekt bedzie tego samego typu, co obiekt stworzony rpze wywolanie klasy (new)
                // nazwaobiektu  new<- zawsze   Sector() czyli z czego ma go budowac          

            Assert.AreEqual(12, testowysector.Id, "Id doesn't match"); //Assert = wbudowana klasa, sluzaca do porownywania czy wartosci zostaly poprawnie zainicjalizowane
            Assert.AreEqual(10, testowysector.FillTreshhold, "FT doesn't match");
            Assert.AreEqual(4, testowysector.Width, "W doesn't match");
            Assert.AreEqual(6, testowysector.Height, "H doesn't match");
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CzekErja()
        {
            var testowysector = new Sector(fillTreshhold: 1, id: 1, height: 5, width: 10, globalX: 0, globalY: 0); // <-- to jest wzorcowy model wywoalania funkcji, nie wkladam po kolei 4x random numer, bo nie wiadomo o co chodzi, tylko po kolei nazywasz i przypisujesz wartosc, ze inna osoba wie o co kaman
            Assert.AreEqual(50, testowysector.Area, "area doesn't match");
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void CzekFillCountIsInnitializedToZerou()
        {
            var testowysecotr = new Sector(fillTreshhold: 1, id: 1, height: 5, width: 10, globalX: 0, globalY: 0);
            Assert.AreEqual(0, testowysecotr.FillCount, "hahaha nie");
        }
        
        [TestMethod]
        [TestCategory("Unit")]
        public void CheckIncreaseFillCount()
        {
            var testowysec = new Sector(fillTreshhold: 1, id: 1, height: 5, width: 10, globalX: 0, globalY: 0);
            testowysec.IncreaseFillCount();
            testowysec.IncreaseFillCount();
            Assert.AreEqual(2, testowysec.FillCount, "should be 2, Jim");
        }
        [TestMethod]
        [TestCategory("Unit")]
        public void TestCzyFillRatioPoprawnieDajeDouble()
        {
            var sut = new Sector(fillTreshhold: 1, id: 1, height: 5, width: 10, globalX: 0, globalY: 0);
            for (int i =0;i<11;i++)
            {
                sut.IncreaseFillCount();
            }
            Assert.AreEqual(11, sut.FillCount, "Fill count is not 11");
            Assert.AreEqual(0.22, sut.FillRatio, "Fill count rato of 11/50 = 0.22");
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void IsBlackDlaCzarnego()
        {
            var sut = new Sector(fillTreshhold: 0.5, id: 1, height: 5, width: 10, globalX: 0, globalY: 0);
            for (int i = 0; i < 27; i++)
            {
                sut.IncreaseFillCount();
            }
            Assert.IsTrue(sut.IsBlack(), "spodziewalem sie czarnucha, a jest bialas");
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void IsBlackDlaBialego()
        {
            var sut = new Sector(fillTreshhold: 0.5, id: 1, height: 5, width: 10, globalX: 0, globalY: 0);
            for (int i = 0; i < 17; i++)
            {
                sut.IncreaseFillCount();
            }
            Assert.IsFalse(sut.IsBlack(), "spodziewalem sie bialasa, a jest NEGROID, FillRatio: "+sut.FillRatio );
        }
    }
}
