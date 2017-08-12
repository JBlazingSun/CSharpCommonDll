using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpCommonDll
{
    public partial class Jyh
    {
        /// <summary>
        /// TXT文件导入Sqlserver
        /// </summary>
        /// <param name="txtFilePath">TXT文件路径</param>
        /// <param name="tableName">SQL表名</param>
        /// <returns>影响行数</returns>
        public int TxtToSqlServer(string txtFilePath, string tableName)
        {
            if (txtFilePath == "")
            {
                return 0;
            }
            int count = 0;
            var querySql = new StringBuilder("");
            var streamReader = new StreamReader(txtFilePath, Encoding.UTF8);
            var line = string.Empty;
            while ((line = streamReader.ReadLine()) != null)
            {
                var strs = line.Split('|');
                querySql.Append($"INSERT INTO {tableName} VALUES (");
                for (int i = 0; i < strs.Count() - 1; i++)
                {
                    if (i == strs.Length - 2)
                    {
                        querySql.Append("'" + strs[i] + "'");
                        break;
                    }
                    querySql.Append("'" + strs[i] + "',");
                }
                querySql.Append(") ");
            }
            string final = querySql.ToString();
            //count += db.Context.FromSql(final).ExecuteNonQuery();
            streamReader.Close();
            return count;
        }

        /// <summary>
        /// Excel导出到Txt
        /// </summary>
        /// <param name="excelpath">文件路径</param>
        /// <param name="sheetName">EXCEL表名</param>
        /// <param name="outPutPath">转换后输出路径</param>
        /// <returns>返回生成TXT文件路径</returns>
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
        /// <summary>
        /// Excel导出到DataTable
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="sheetName"></param>
        /// <returns>DataTable</returns>
        public static DataTable ExcelSheetToDataTable(string fileName, string sheetName)
        {
            OleDbConnection oleDb = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileName +
                                                        ";Extended Properties=Excel 12.0");
            oleDb.Open();

            OleDbDataAdapter oleDbAdapter = new OleDbDataAdapter("select * from [" + sheetName + "$]", oleDb);

            DataTable mydt = new DataTable();
            oleDbAdapter.Fill(mydt);
            return mydt;
        }

        /// <summary>
        /// 获取excel的所有sheet名称
        /// </summary>
        /// <param name="filepath"></param>
        /// <returns></returns>
        public static StringCollection ExcelSheetName(string filepath)
        {
            StringCollection names = new StringCollection();
            string strConn;
            strConn = "Provider=Microsoft.Ace.OLEDB.12.0;Data Source=" + filepath +
                      ";Extended Properties='Excel 12.0;HDR=Yes;IMEX=2'";
            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();
            DataTable sheetNames = conn.GetOleDbSchemaTable
                (System.Data.OleDb.OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            conn.Close();

            foreach (DataRow dr in sheetNames.Rows)
            {
                string thisName = dr[2].ToString();
                if (thisName.EndsWith("$"))
                {
                    thisName = thisName.Substring(0, thisName.Length - 1);
                }
                names.Add(thisName);
            }
            return names;
        }

        /// <summary>
        /// DataTable导出到CSV
        /// </summary>
        /// <param name="dvThis"></param>
        /// <returns></returns>
        public bool DataTableToCsv(DataView dvThis)
        {
            try
            {
                var saveFileName = OpenFile();
                // string fileName = saveFileDialog.FileName;//文件名字
                using (StreamWriter streamWriter = new StreamWriter(saveFileName, false, Encoding.Default))
                {
                    StringBuilder sb = new StringBuilder();
                    ///写入标题
                    int iColCount = dvThis.Table.Columns.Count;
                    for (int i = 0; i < iColCount; i++)
                    {
                        sb.Append(dvThis.Table.Columns[i].ColumnName)
                            .Append(","); //没一个字段后面都加逗号，表示是一列，因为这是第一行    因此也是列标题
                    }
                    streamWriter.WriteLine(sb.ToString());
                    ///写入表格
                    foreach (DataRowView dr in dvThis)
                    {
                        for (int i = 0; i < iColCount; i++)
                        {
                            if (!Convert.IsDBNull(dr[i]))
                                streamWriter.Write(dr[i].ToString());
                            else
                                streamWriter.Write("\"\"");
                            if (i < iColCount - 1)
                            {
                                streamWriter.Write(",");
                            }
                        }
                        streamWriter.Write(streamWriter.NewLine);
                    }
                    streamWriter.Flush();
                    streamWriter.Close();
                    var rd = MessageBox.Show("是否打开文件", "导出成功!", MessageBoxButtons.YesNo);
                    if (rd == DialogResult.Yes | rd == DialogResult.OK)
                    {
                        SaveFile(saveFileName);
                    }
                    return true;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        /// <summary>
        /// datatable导出到excel,
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="sheetName"></param>
        /// <param name="showWindowsOrSave">true代表直接显示excel,将不会保存本地文件;false代表用户必须选择保存本地文件,保存成功后会提示用户是否打开文件</param>
        public void DataTableToExcel(DataTable[] dt, string[] sheetName, bool showWindowsOrSave = false)
        {
            if (dt.Length != sheetName.Length)
            {
                MessageBox.Show("传入的前两个参数的个数必须完全一致");
                return;
            }
            Microsoft.Office.Interop.Excel.Application myExcel = new Microsoft.Office.Interop.Excel.Application();
            myExcel.ShowWindowsInTaskbar = true;
            myExcel.Visible = false;
            Microsoft.Office.Interop.Excel.Workbook Xlsbook = myExcel.Workbooks.Add();
            //删除多余sheet
            for (int i = 1; i < Xlsbook.Sheets.Count; i++)
            {
                Xlsbook.Sheets[i].delete();
            }

            for (int i = 0; i < dt.Length; i++)
            {
                DataTable DT_This = dt[i];
                string[,] dataarray = new string[DT_This.Rows.Count + 1, DT_This.Columns.Count];
                Microsoft.Office.Interop.Excel.Worksheet Xlssheet = (i == 0 ? Xlsbook.Sheets[1] : Xlsbook.Sheets.Add());
                Xlssheet.Name = sheetName[i];
                for (int j = 0; j < DT_This.Columns.Count; j++)
                {
                    dataarray[0, j] = DT_This.Columns[j].ColumnName;
                    for (int m = 0; m < DT_This.Rows.Count; m++)
                    {
                        dataarray[m + 1, j] = DT_This.Rows[m][j].ToString();
                    }
                }
                Xlssheet.Range["A1"].Resize[dataarray.GetLength(0), dataarray.GetLength(1)].Value = dataarray;
            }
            if (showWindowsOrSave)
            {
                myExcel.Visible = true;
            }
            else
            {
                string filename = SaveFile();
                if (filename != "")
                {
                    try
                    {
                        Xlsbook.SaveAs(filename);
                        myExcel.Quit();
                        //关闭excel
                        try
                        {
                            if (myExcel != null)
                            {
                                int lpdwProcessId;
                                GetWindowThreadProcessId(new IntPtr(myExcel.Hwnd), out lpdwProcessId);
                                System.Diagnostics.Process.GetProcessById(lpdwProcessId).Kill();
                            }
                        }
                        catch (Exception ex)
                        {
                            //  Console.WriteLine("Delete Excel Process Error:" + ex.Message);
                        }
                        var rd = MessageBox.Show("是否打开文件", "导出成功!", MessageBoxButtons.YesNo);
                        if (rd == DialogResult.Yes | rd == DialogResult.OK)
                        {
                            Process.Start(filename);
                        }
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show(e.Message);
                    }

                }
            }
        }
    }
}
