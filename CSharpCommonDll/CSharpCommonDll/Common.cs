using System.Windows.Forms;

namespace CSharpCommonDll
{
    public partial class Jyh
    {
        private static readonly Jyh _instance = new Jyh();

        private Jyh()
        {
        }

        public static Jyh GetInstance()
        {
            return _instance;
        }

        //确定对话框
        public bool ConfirmForm(string content, string title = "确认?")
        {
            var messButton = MessageBoxButtons.OKCancel;
            var dr = MessageBox.Show(content, title, messButton);
            if (dr == DialogResult.Cancel)
                return false;
            return true;
        }
    }
}