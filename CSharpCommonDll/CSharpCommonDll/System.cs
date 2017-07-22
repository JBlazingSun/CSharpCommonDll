using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCommonDll
{
    public partial class Jyh
    {
        //取第一块硬盘编号   
        public String GetHardDiskId()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
                String strHardDiskId = null;
                foreach (var o in searcher.Get())
                {
                    var mo = (ManagementObject) o;
                    strHardDiskId = mo["SerialNumber"].ToString().Trim();
                    break;
                }
                return strHardDiskId;
            }
            catch
            {
                return "";
            }
        }
    }
}
