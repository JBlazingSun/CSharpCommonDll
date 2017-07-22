using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCommonDll
{
    public partial class Jyh
    {
        /// <summary>
        /// TXT文件导入Sqlserver生成的SQL
        /// </summary>
        /// <param name="txtFilePath">TXT文件路径</param>
        /// <param name="tableName">SQL表名</param>
        /// <returns>要执行插入的SQL语句</returns>
        public string TxtToSqlServer(string txtFilePath, string tableName)
        {
            if (txtFilePath == "")
            {
                return string.Empty;
            }
            var streamReader = new StreamReader(txtFilePath, Encoding.UTF8);
            var line = string.Empty;
            var querySql = string.Empty;
            while ((line = streamReader.ReadLine()) != null)
            {
                var strs = line.Split('|');
                querySql = $"INSERT INTO {tableName} VALUES (";
                for (int i = 0; i < strs.Count(); i++)
                {
                    querySql += "'" + strs[i] + "'";
                }
                querySql += ")";
                //count += db.Context.FromSql(querySql).ExecuteNonQuery();
            }
            return querySql;
        }

        /// <summary>
        /// 返回生成TXT文件路径
        /// </summary>
        /// <param name="excelpath">文件路径</param>
        /// <param name="sheetName">EXCEL表名</param>
        /// <param name="outPutPath">转换后输出路径</param>
        /// <returns>输出文件完整路径</returns>
        public string ExcelToTxt(string excelpath, string sheetName, string outPutPath)
        {
            if (excelpath == "null")
            {
                return "";
            }
            OleDbConnection oleDb = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + excelpath + ";Extended Properties=Excel 12.0");
            oleDb.Open();

            OleDbDataAdapter oleDbAdapter = new OleDbDataAdapter("select * from [" + sheetName + "$]", oleDb);

            DataSet myds = new DataSet();
            oleDbAdapter.Fill(myds);

            //写入
            StreamWriter streamWriter = new StreamWriter(outPutPath + sheetName + ".txt", true, Encoding.UTF8);
            for (int i = 0; i < myds.Tables[0].Rows.Count; i++)
            {
                for (int j = 0; j < myds.Tables[0].Columns.Count; j++)
                {
                    streamWriter.Write(myds.Tables[0].Rows[i][j].ToString() + "|");
                }
                streamWriter.Write(Environment.NewLine);
            }
            streamWriter.Close();
            streamWriter.Dispose();
            return outPutPath + "\\" + sheetName + ".txt";
        }
    }
}
