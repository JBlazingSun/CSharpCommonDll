using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CSharpCommonDll
{
    public partial class Jyh
    {
        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="password"></param>
        /// <param name="ciphertext">已加密的字串</param>
        /// <returns></returns>
        public string DESDecrypt(string ciphertext,string password,string password2="blazings")
        {
            string cipher = string.Empty;

            try
            {
                char[] key = new char[8];
                if (password.Length > 8)
                {
                    password = password.Remove(8);
                }
                password.CopyTo(0, key, 0, password.Length);

                char[] iv = new char[8];
                if (password2.Length > 8)
                {
                    password2 = password2.Remove(8);
                }
                password2.CopyTo(0, iv, 0, password2.Length);

                if (ciphertext == null)
                {
                    return cipher;
                }

                SymmetricAlgorithm serviceProvider = new DESCryptoServiceProvider();
                serviceProvider.Key = Encoding.ASCII.GetBytes(key);
                serviceProvider.IV = Encoding.ASCII.GetBytes(iv);

                byte[] contentArray = Convert.FromBase64String(ciphertext);
                MemoryStream memoryStream = new MemoryStream(contentArray);
                CryptoStream cryptoStream = new CryptoStream(memoryStream, serviceProvider.CreateDecryptor(), CryptoStreamMode.Read);
                StreamReader streamReader = new StreamReader(cryptoStream);

                cipher = streamReader.ReadToEnd();

                streamReader.Dispose();
                cryptoStream.Dispose();
                memoryStream.Dispose();
                serviceProvider.Clear();

            }
            catch (Exception ex)
            {
                Console.Write("密钥错误,数据包解密失败.");
            }
            return cipher;
        }

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="password">自己定义</param>
        /// <param name="text">加密字串</param>
        /// <param name="password2">自己定义</param>
        /// <returns></returns>
        public string DESEncrypt(string text, string password, string password2="blazings")
        {
            string cipher;
            char[] key = new char[8];
            if (password.Length > 8)
            {
                password = password.Remove(8);
            }
            password.CopyTo(0, key, 0, password.Length);

            char[] iv = new char[8];
            if (password2.Length > 8)
            {
                password2 = password2.Remove(8);
            }
            password2.CopyTo(0, iv, 0, password2.Length);

            if (text == null)
            {
                return string.Empty;
            }

            SymmetricAlgorithm serviceProvider = new DESCryptoServiceProvider();
            serviceProvider.Key = Encoding.ASCII.GetBytes(key);
            serviceProvider.IV = Encoding.ASCII.GetBytes(iv);

            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, serviceProvider.CreateEncryptor(), CryptoStreamMode.Write);
            StreamWriter streamWriter = new StreamWriter(cryptoStream);

            streamWriter.Write(text);
            streamWriter.Dispose();
            cryptoStream.Dispose();

            byte[] signData = memoryStream.ToArray();
            memoryStream.Dispose();
            serviceProvider.Clear();
            cipher = Convert.ToBase64String(signData);

            return cipher;
        }
    }
}