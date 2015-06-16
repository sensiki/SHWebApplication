using System;
using System.Collections.Generic;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.SessionState;
using System.IO;
using cn.com.farsight.SH.SHDbVisit;
using cn.com.farsight.SH.SHModel;
using Newtonsoft.Json;

namespace cn.com.farsight.SH.SHWebApplication.Admin
{
    /// <summary>
    /// admin 的摘要说明
    /// </summary>
    public class admin : IHttpHandler, IRequiresSessionState
    {
        DataManager dm = new DataManager();
        CommandManager cm = new CommandManager();
        StatusManager sm = new StatusManager();
        TemHumManager thm = new TemHumManager();
        UserManager um = new UserManager();
        public void ProcessRequest(HttpContext context)
        {
            if (context.Request.RequestType == "GET")
            {
                context.Response.ContentType = "text/html";
                if (context.Session["LoginUserName"] == null || string.IsNullOrEmpty(context.Session["LoginUserName"].ToString()))
                    context.Response.Write("<script>alert(\"权限不足,请重新登录!\");window.location.href=\"login.ashx\"</script>");
                else
                {
                    context.Response.ContentType = "text/html";
                    StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "html/dashboard.html");
                    string html = reader.ReadToEnd();
                    reader.Close();
                    context.Response.Write(html);
                }
            }
            else
            {
                if (context.Request["type"] == "flot-TemHum")
                {
                    List<NodeData> list = new List<NodeData>();
                    NodeData n = new NodeData();
                    list = thm.getModelListTop(n, 1);
                    context.Response.Write(JsonConvert.SerializeObject(list));
                }
                else if (context.Request["type"] == "GetUserName")
                {
                    string a = context.Session["LoginUserName"].ToString();
                    context.Response.Write(context.Session["LoginUserName"].ToString());
                }

                else if (context.Request["type"] == "updata")
                {
                    int count = 0;
                    NodeStatus ns = new NodeStatus();
                    if ((count = sm.GetRecordCount(ns)) > 0)
                    {
                        List<NodeStatus> list = sm.getModelListAll(ns);
                        context.Response.Write(JsonConvert.SerializeObject(list));
                    }
                    else
                        context.Response.Write("false");
                }
                else if (context.Request["type"] == "light")
                {
                    if (context.Session["LoginUserPermisson"] != null && context.Session["LoginUserPermisson"].ToString() != "0")
                    {
                        User u = new User();
                        u.Name = context.Session["LoginUserName"].ToString();
                        um.getModel(u);
                        string userid = u.Id.ToString();
                        if (context.Request["node"] == "OneFL")
                        {
                            if (SendCommand("01", "72", userid))
                                context.Response.Write("true");
                            else
                                context.Response.Write("false");
                        }
                        else if (context.Request["node"] == "TwoFL")
                        {
                            if (SendCommand("02", "72", userid))
                                context.Response.Write("true");
                            else
                                context.Response.Write("false");
                        }
                        else if (context.Request["node"] == "Hall")
                        {
                            if (SendCommand("03", "72", userid))
                                context.Response.Write("true");
                            else
                                context.Response.Write("false");
                        }
                        else if (context.Request["node"] == "Stairs")
                        {
                            if (SendCommand("04", "72", userid))
                                context.Response.Write("true");
                            else
                                context.Response.Write("false");
                        }
                        else if (context.Request["node"] == "Kitchen")
                        {
                            if (SendCommand("05", "72", userid))
                                context.Response.Write("true");
                            else
                                context.Response.Write("false");
                        }
                        else if (context.Request["node"] == "Study")
                        {
                            if (SendCommand("06", "72", userid))
                                context.Response.Write("true");
                            else
                                context.Response.Write("false");
                        }
                    }
                    else
                        context.Response.Write("low");
                }
            }
           
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        private bool SendCommand(string modle1,string modle2,string userid)
        {
            NodeStatus s = new NodeStatus();
            s.Modle = modle1;
            s = sm.GetListByPage(s);
            if (s != null && s.Time.CompareTo(DateTime.Now.AddSeconds(-10)) >= 0 && s.Time.CompareTo(DateTime.Now.AddSeconds(10)) <= 0)
            {
                byte[] array = new byte[1];
                NodeCommand n = new NodeCommand();
                n.Identify = "2343";
                n.Type = s.Type;
                n.Modle = modle2;
                n.User = userid;
                n.Addr = s.Addr.Substring(2, 2) + s.Addr.Substring(0, 2); 
                if (s.Data == "000001")
                    n.Data = "30";
                else
                    n.Data = "31";
                n.Time = DateTime.Now;
                cm.addModel(n);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}