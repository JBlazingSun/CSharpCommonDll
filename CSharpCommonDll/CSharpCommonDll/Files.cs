using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Microsoft.Win32;

namespace CSharpCommonDll
{
    public partial class Jyh
    {
        //section：要读取的段落名
        //    key: 要读取的键
        //    defVal: 读取异常的情况下的缺省值
        //    retVal: key所对应的值，如果该key不存在则返回空值
        //    size: 值允许的大小
        //    filePath: INI文件的完整路径和文件名
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string defVal,
            StringBuilder retVal, int size, string filePath);

        //section: 要写入的段落名
        //    key: 要写入的键，如果该key存在则覆盖写入
        //    val: key所对应的值
        //    filePath: INI文件的完整路径和文件名
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        /// 读取INI文件
        /// </summary>
        /// <param name="section">项目名称(如 [section] )</param>
        /// <param name="skey">键</param>
        /// <param name="path">路径</param>
        public string IniReadValue(string section, string skey, string path)
        {
            var temp = new StringBuilder(5000);
            var i = GetPrivateProfileString(section, skey, "", temp, 5000, path);
            return temp.ToString();
        }

        /// <summary>
        ///     写入ini文件
        /// </summary>
        /// <param name="section">项目名称</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="path">路径</param>
        public void IniWrite(string section, string key, string value, string path)
        {
            WritePrivateProfileString(section, key, value, path);
        }

        /// <summary>
        ///     写入txt文件, 会替换原来内容
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="textValue"></param>
        public void WriteTxt(string filePath, string textValue)
        {
            var sw = new StreamWriter(filePath);
            var value = textValue;
            sw.Write(value);
            sw.Close();
        }

        /// <summary>
        ///     追加写入txt文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="textValue"></param>
        public void WriteTxtAppend(string filePath, string textValue, bool isNewLine = false)
        {
            var sw = File.AppendText(filePath);
            var value = textValue;
            sw.Write(value);
            if (isNewLine)
                sw.Write(Environment.NewLine);
            sw.Close();
        }

        /// <summary>
        ///     修改txt文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="oldText"></param>
        /// <param name="textValue"></param>
        public void ModifyTxt(string filePath, string oldText, string textValue)
        {
            var list = ReadTxt(filePath);
            var str = string.Join(Environment.NewLine, list.ToArray());
            var strSp = str.Replace(oldText, textValue);
            WriteTxt(filePath, strSp);
        }

        /// <summary>
        ///     读取txt,返回每一行
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public List<string> ReadTxt(string filePath)
        {
            var list = new List<string>();
            var objReader = new StreamReader(filePath, Encoding.Default);
            var sLine = "";
            while (!objReader.EndOfStream)
            {
                sLine = objReader.ReadLine();
                if (sLine != null && !sLine.Equals(""))
                    list.Add(sLine); //+ Environment.NewLine
            }
            objReader.Close();
            return list;
        }

        /// <summary>
        ///     保存文件
        /// </summary>
        /// <param name="fileFilter"></param>
        /// <returns>文件完整路径</returns>
        public string SaveFile(string fileFilter = "")
        {
            var savefilepath = Directory.GetCurrentDirectory();
            var ofd = new SaveFileDialog
            {
                Filter = fileFilter + "所有文件|*.*",
                ValidateNames = true,
                CheckPathExists = true,
                CheckFileExists = true,
                InitialDirectory = Directory.GetCurrentDirectory(),
                RestoreDirectory = true
            };
            if (ofd.ShowDialog() == DialogResult.OK)
                savefilepath = ofd.FileName;
            return savefilepath;
        }

        /// <summary>
        ///     打开文件
        /// </summary>
        /// <returns>文件完整路径</returns>
        public string OpenFile(string fileFilter = "")
        {
            var openFilePath = "null";
            var ofd = new OpenFileDialog
            {
                Filter = fileFilter + "所有文件|*.*",
                ValidateNames = true,
                CheckPathExists = true,
                CheckFileExists = true,
                InitialDirectory = Directory.GetCurrentDirectory(),
                RestoreDirectory = true
            };
            if (ofd.ShowDialog() == DialogResult.OK)
                openFilePath = ofd.FileName;
            return openFilePath;
        }

        /// <summary>
        ///     写入注册表
        /// </summary>
        /// <param name="mainKey"></param>
        /// <param name="subKey"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool WriteRegister(string mainKey, string subKey, string key, string value)
        {
            // Create a subkey named Test9999 under HKEY_CURRENT_USER.
            var reg = Registry.CurrentUser.CreateSubKey(mainKey);
            // Create two subkeys under HKEY_CURRENT_USER\Test9999. 
            if (reg == null)
                return false;
            var setting = reg.CreateSubKey(subKey);
            // Create data for the TestSettings subkey.
            if (setting != null) setting.SetValue(key, value);
            else
                return false;
            return true;
        }

        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="mainKey"></param>
        /// <param name="subKey"></param>
        /// <returns></returns>
        public Dictionary<string, string> GetRegister(string mainKey, string subKey)
        {
            var regInfo = new Dictionary<string, string>();
            var reg = Registry.CurrentUser.CreateSubKey(mainKey);

            foreach (var subKeyName in reg.GetSubKeyNames())
            {
                var tempKey = reg.OpenSubKey(subKeyName);
                foreach (var valueName in tempKey.GetValueNames())
                    regInfo.Add(valueName, tempKey.GetValue(valueName).ToString());
            }
            return regInfo;
        }

        /// <summary>
        ///     删除子项和子级子项
        /// </summary>
        /// <param name="mainKey"></param>
        /// <returns></returns>
        public bool DeleteRegisterMainKeyTree(string mainKey)
        {
            try
            {
                Registry.CurrentUser.DeleteSubKeyTree(mainKey);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 复制文件夹（及文件夹下所有子文件夹和文件）
        /// </summary>
        /// <param name="sourcePath">待复制的文件夹路径</param>
        /// <param name="destinationPath">目标路径</param>
        public static void CopyDirectory(String sourcePath, String destinationPath)
        {
            DirectoryInfo info = new DirectoryInfo(sourcePath);
            Directory.CreateDirectory(destinationPath);
            foreach (FileSystemInfo fsi in info.GetFileSystemInfos())
            {
                String destName = Path.Combine(destinationPath, fsi.Name);

                if (fsi is System.IO.FileInfo)          //如果是文件，复制文件
                    File.Copy(fsi.FullName, destName);
                else                                    //如果是文件夹，新建文件夹，递归
                {
                    Directory.CreateDirectory(destName);
                    CopyDirectory(fsi.FullName, destName);
                }
            }
        }
        /// <summary>
        /// 删除文件夹（及文件夹下所有子文件夹和文件）
        /// </summary>
        /// <param name="directoryPath"></param>
        public static void DeleteFolder(string directoryPath)
        {
            foreach (string d in Directory.GetFileSystemEntries(directoryPath))
            {
                try
                {
                    if (File.Exists(d))
                    {
                        FileInfo fi = new FileInfo(d);
                        if (fi.Attributes.ToString().IndexOf("ReadOnly", StringComparison.Ordinal) != -1)
                            fi.Attributes = FileAttributes.Normal;
                        File.Delete(d);     //删除文件   
                    }
                    else
                    {
                        DeleteFolder(d);    //删除文件夹
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    throw;
                }
            }
            //Directory.Delete(directoryPath);    //删除空文件夹
        }
    }
}