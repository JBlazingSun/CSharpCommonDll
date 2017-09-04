using CSharpCommonDll;
using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CSharpCommonDll.Tests
{
    [TestClass()]
    public class UnitTest1
    {
        private Jyh jyh = Jyh.GetInstance();
        private string mainkey = "mainkey";
        private string subkey = "subkey";
        private string key = "key";
        private string value = "value";
        [TestMethod()]
        public void WriteRegisterTest()
        {
            Assert.AreEqual(true, jyh.WriteRegister(mainkey, subkey,key, value));
        }

        [TestMethod()]
        public void GetRegisterTest()
        {
            var regInfo = jyh.GetRegister(mainkey, subkey);
            Assert.AreEqual(key,regInfo.Keys.First());
            Assert.AreEqual(value,regInfo.Values.First());
        }
    }
}

