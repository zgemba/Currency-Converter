using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Zgemba.Utils;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void ConverterTest()
        {
            CurrencyConverter cvt = CurrencyConverter.Instance;
            decimal value = 100m;

            Assert.AreEqual(cvt.ConvertToEuro(cvt.ConvertFromEuro(value, "USD"), "USD"), value);
        }
    }
}
