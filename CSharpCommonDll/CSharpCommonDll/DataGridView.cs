using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCommonDll
{
    public partial class DataGridView
    {
        ///   <summary> 
        ///   比较两个字段集合是否名称,数据类型一致 
        ///   </summary> 
        ///   <param   name= "dcA "> </param> 
        ///   <param   name= "dcB "> </param> 
        ///   <returns> </returns> 
        private bool CompareColumn(DataColumnCollection dcA, DataColumnCollection dcB)
        {
            if (dcA.Count != dcB.Count) return false;
            foreach (DataColumn dc in dcA)
            {
                //找相同字段名称 
                if (dcB.IndexOf(dc.ColumnName) > -1)
                {
                    //测试数据类型 
                    if (dc.DataType != dcB[dcB.IndexOf(dc.ColumnName)].DataType)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        ///   <summary>
        ///   比较两个DataTable内容是否相等，先是比数量，数量相等就比内容 
        ///   </summary> 
        ///   <param   name= "dtA "> </param> 
        ///   <param   name= "dtB "> </param> 
        private bool CompareDataTable(DataTable dtA, DataTable dtB)
        {
            if (dtA == null || dtB == null)
            {
                return false;
            }
            if (dtA.Rows.Count != dtB.Rows.Count) return false;
            if (!CompareColumn(dtA.Columns, dtB.Columns)) return false;
            //比内容 
            for (var i = 0; i < dtA.Rows.Count; i++)
            {
                for (int j = 0; j < dtA.Columns.Count; j++)
                {
                    if (!dtA.Rows[i][j].Equals(dtB.Rows[i][j]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
