using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpCommonDll
{
    public class Network
    {
        public bool TestInternet(string url = "114.114.114.114", int timeout = 2)
        {
            try
            {
                Ping ping = new Ping();
                PingReply pingReply = ping.Send(url, timeout);
                if (pingReply.Status == IPStatus.Success)
                {
                    //MessageBox.Show(pingReply.Status.ToString());
                    return true;
                }
                else
                {
                    //MessageBox.Show(pingReply.Status.ToString());
                    return false;
                }
            }
            finally
            {

            }
        }
        //测试SqlServer
        public bool TestSqlConn(string connStr, int timeOut = 2000)
        {
            string[] s = connStr.Split(';');
            s = s[0].Split('=');
            //获取IP 
            string strIP = s[1];
            if (TestConnection(strIP, 1433, timeOut))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary> 
        /// 采用Socket方式，测试服务器连接 
        /// </summary> 
        /// <param name="host">服务器主机名或IP</param> 
        /// <param name="port">端口号</param> 
        /// <param name="millisecondsTimeout">等待时间：毫秒</param> 
        /// <returns></returns> 
        public static bool TestConnection(string host, int port = 1433, int millisecondsTimeout = 2000)
        {
            TcpClient client = new TcpClient();
            try
            {
                var ar = client.BeginConnect(host, port, null, null);
                ar.AsyncWaitHandle.WaitOne(millisecondsTimeout);
                return client.Connected;
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                client.Close();
            }
        }
    }
}
