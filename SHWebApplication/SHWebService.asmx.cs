using cn.com.farsight.SH.SHDbVisit;
using cn.com.farsight.SH.SHModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.Services;

namespace SHWebApplication
{
    /// <summary>
    /// SHWebApplication 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class SHWebService : System.Web.Services.WebService
    {
        DataManager dm = new DataManager();
        CommandManager cm = new CommandManager();
        StatusManager sm = new StatusManager();
        TemHumManager thm = new TemHumManager();
        UserManager um = new UserManager();
        CommandLogManager clm = new CommandLogManager();
        [WebMethod]
        public string SerPut(string data)
        {
            if (data.Length == 264)
            {
                string[] nodedata = new string[12];
                for(int i=0;i<12;i++)
                {
                    nodedata[i] = data.Substring(22*i, 22);
                    if (nodedata[i].Substring(0, 2) != "00")
                    {
                        NodeStatus a = Data_Conversion(nodedata[i]);
                        int b = Convert.ToInt32(a.Modle, 16);
                        if (b > 0 && b < 13)
                        {
                            if (b == 11)
                            {
                                tem_hum_put(a);
                            }
                            if (!sm.updateModel(a))
                                sm.addModel(a);
                        }
                    }
                }
                return "ok";
            }
            else
            {
                return "error3";
            }
        }
        [WebMethod]
        public string CliPut(string data)
        {
            if (data.Length > 14)
            {
                NodeCommand p = new NodeCommand();
                while (cm.GetRecordCount(p) > 10)
                {
                    cm.deleteModel(cm.GetListByPage(p));
                }
                p = Command_Conversion(data);
                command_log_put(p);
                if (cm.addModel(p))
                    return "ok";
                else
                    return "fail";
            }
            else
                return "error1";
        }
        [WebMethod]
        public string SerGet()
        {
            int count = 0;
            NodeCommand p = new NodeCommand();
            if ((count = cm.GetRecordCount(p)) > 0)
            {
                p = cm.GetListByPage(p);
                while (p.Time.CompareTo(DateTime.Now.AddSeconds(-10)) < 0 || p.Time.CompareTo(DateTime.Now.AddMinutes(10)) > 0)
                {
                    cm.deleteModel(p);
                    NodeCommand q = new NodeCommand();
                    if ((count = cm.GetRecordCount(q)) > 0)
                    {
                        p = cm.GetListByPage(q);
                    }
                    else
                    {
                        return null;
                    }
                }
                string a = p.Identify + p.Type + p.Modle + p.Addr + p.Data;
                a = hexStr2Str(a);
                cm.deleteModel(p);
                return a;
            }
            return null;
        }
        [WebMethod]
        public string CliGet()
        {
            NodeStatus ns = new NodeStatus();
            bool isnull = true;
            string[] a = new string[12];
            List<NodeStatus> list = sm.getModelListAll(ns);
            for (int i = 0; i < list.Count; i++)
            {
                int b = Convert.ToInt32(list[i].Modle, 16);
                if ((b > 0 && b < 13) && (list[i].Time.CompareTo(DateTime.Now.AddSeconds(-10)) >= 0 && list[i].Time.CompareTo(DateTime.Now.AddSeconds(10)) <= 0))
                {
                    string c = list[i].Type + list[i].Addr + list[i].Modle + list[i].Data + list[i].IP + list[i].Battery + list[i].Checksum;
                    c = hexStr2Str(c);
                    a[b - 1] = c;
                    isnull = false;
                }
            }
            string f = null;
            for (int i = 0; i < 12; i++)
            {
                if (a[i] != null)
                {
                    if (a[i].Length == 11)
                        f += a[i];
                    else
                        f += hexStr2Str("0000000000000000000000");
                }
                else
                {
                    f += hexStr2Str("0000000000000000000000");
                }
            }
            if (isnull)
                return "null";
            return f;
        }
        [WebMethod]
        public string UserCheck(string name, string pwd)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(pwd))
            {
                return "null";
            }
            User d = new User() { Name = name };

            if (um.getModel(d))//根据name查找数据库，把符合name的用户信息传递到变量d并返回true，否则返回false
            {
                if (d.Pwd.CompareTo(pwd) != 0)
                {
                    return "error2";
                }
                else
                {
                    um.updateModel(d);
                    return d.Id.ToString(); 
                }
            }
            else
            {
                return "error1"; ;
            }
        }
        private bool tem_hum_put(NodeStatus data)
        {
            NodeData p = new NodeData();
            while (thm.GetRecordCount(p) > 24)
            {
                thm.deleteModel(thm.GetListByPage(p, false));
            }
            p.Type = data.Type;
            p.Addr = data.Addr;
            p.Modle = data.Modle;
            p.Data = data.Data;
            p.IP = data.IP;
            p.Battery = data.Battery;
            p.Checksum = data.Checksum;
            p.Time = data.Time;
            if (thm.GetListByPage(p, false) == null)
            {
                return thm.addModel(p);
            }
            else
                return false;

        }
        private bool command_log_put(NodeCommand data)
        {
            NodeCommand p = new NodeCommand();
            while (clm.GetRecordCount(p) > 10)
            {
                clm.deleteModel(clm.GetListByPage(p));
            }
            return clm.addModel(data);
        }
        private NodeData Data_Conversion(byte[] data)
        {
            NodeData nd = new NodeData();
            nd.Type = String.Format("{0:X2}", data[0]);
            nd.Addr = String.Format("{0:X2}", data[1]) + String.Format("{0:X2}", data[2]);
            nd.Modle = String.Format("{0:X2}", data[3]);
            nd.Data = String.Format("{0:X2}", data[4]) + String.Format("{0:X2}", data[5]) + String.Format("{0:X2}", data[6]);
            nd.IP = String.Format("{0:X2}", data[7]) + String.Format("{0:X2}", data[8]);
            nd.Battery = String.Format("{0:X2}", data[9]);
            nd.Checksum = String.Format("{0:X2}", data[10]);
            nd.Time = DateTime.Now;
            return nd;
        }
        private NodeCommand Command_Conversion(byte[] data)
        {
            NodeCommand nc = new NodeCommand();
            nc.Identify = System.Text.ASCIIEncoding.Default.GetString(data, 0, 2);
            nc.Type = System.Text.ASCIIEncoding.Default.GetString(data, 2, 1);
            nc.Modle = System.Text.ASCIIEncoding.Default.GetString(data, 3, 1);
            nc.Addr = String.Format("{0:X2}", data[4]) + String.Format("{0:X2}", data[5]);
            nc.Data = System.Text.ASCIIEncoding.Default.GetString(data, 6, 1);
            nc.Time = DateTime.Now;
            return nc;
        }
        private NodeStatus Data_Conversion(string data)
        {
            NodeStatus nd = new NodeStatus();
            nd.Type = data.Substring(0, 2);
            nd.Addr = data.Substring(2, 4);
            nd.Modle = data.Substring(6, 2);
            nd.Data = data.Substring(8, 6);
            nd.IP = data.Substring(14, 4);
            nd.Battery = data.Substring(18, 2);
            nd.Checksum = data.Substring(20, 2);
            nd.Time = DateTime.Now;
            return nd;
        }
        private NodeCommand Command_Conversion(string data)
        {
            NodeCommand nc = new NodeCommand();
            nc.Identify = data.Substring(0, 4);
            nc.Type = data.Substring(4, 2);
            nc.Modle = data.Substring(6, 2);
            nc.Addr = data.Substring(8, 4);
            nc.Data = data.Substring(12, 2);
            nc.User = data.Substring(14, data.Length - 14);
            nc.Time = DateTime.Now;
            return nc;
        }
        private String hexStr2Str(String hexStr)
        {
            String str = "0123456789ABCDEF";
            char[] hexs = hexStr.ToCharArray();
            char[] chars = new char[hexStr.Length / 2];   
            string a=null;
            int n;

            for (int i = 0; i < chars.Length; i++)
            {
                n = str.IndexOf(hexs[2 * i]) * 16;
                n += str.IndexOf(hexs[2 * i + 1]);
                chars[i] = (char)n;  
            }
            return new String(chars);
        }     
        private void InitializeComponent()
        {

        }
    }
}
