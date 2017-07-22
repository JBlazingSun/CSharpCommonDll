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
                return (from ManagementObject mo in searcher.Get() select mo["SerialNumber"].ToString().Trim()).FirstOrDefault();
            }
            catch
            {
                return "";
            }
        }
    }
}
