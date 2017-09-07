using System;
using CSharpCommonDll;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;


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
        private string iv = "iv";
        private string password = "string";
        [TestMethod()]
        public void WriteRegisterTest()
        {
            Assert.AreEqual(true, jyh.WriteRegister(mainkey, subkey, key, value));
            Assert.AreEqual(true, jyh.WriteRegister(mainkey, subkey, "aa", "bb"));
        }

        [TestMethod()]
        public void GetRegisterTest()
        {
            var regInfo = jyh.GetRegister(mainkey, subkey);
            Assert.AreEqual(key, regInfo.Keys.First());
            Assert.AreEqual(value, regInfo.Values.First());
        }

        [TestMethod()]
        public void DeleteRegisterTest()
        {
            Assert.AreEqual(true, jyh.DeleteRegisterMainKeyTree("mainkey"));
        }

        [TestMethod()]
        public void DESDecryptTest()
        {
            var s = "nmcCyYI09j8=";
            var deencrypt = jyh.DESDecrypt(s, password);
        }

        [TestMethod()]
        public void DESEncryptTest()
        {
            var encrypt = jyh.DESEncrypt("string", password);
        }

        [TestMethod()]
        public void WriteTxtTest()
        {
            jyh.WriteTxt("C:\\blazings\\application\\pwd.txt", "test");
        }

        [TestMethod()]
        public void WriteTxtAppendTest()
        {
            jyh.WriteTxtAppend("C:\\blazings\\application\\pwd.txt", "test", true);
        }

        [TestMethod()]
        public void ModifyTxtTest()
        {
            jyh.ModifyTxt("C:\\blazings\\application\\pwd.txt", "Collections.", "1");
        }

        [TestMethod()]
        public void ReadTxtTest()
        {
            var v = jyh.ReadTxt("C:\\blazings\\application\\pwd.txt");

        }
    }
}

