using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpCommonDll
{
    public partial class Jyh
    {
        //确定对话框
        public bool ConfirmForm(string content, string title = "确认?")
        {
            MessageBoxButtons messButton = MessageBoxButtons.OKCancel;
            DialogResult dr = MessageBox.Show(content, title, messButton);
            if (dr == DialogResult.Cancel)
                return false;
            else
                return true;
        }
    }
}
