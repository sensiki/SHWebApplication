using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using cn.com.farsight.SH.SHModel;
using cn.com.farsight.SH.SHDbVisit;
using System.Text;
using Newtonsoft.Json;
using System.Web.SessionState;

namespace cn.com.farsight.SH.SHWebApplication.Admin
{
    /// <summary>
    /// log 的摘要说明
    /// </summary>
    public class log : IHttpHandler, IRequiresSessionState
    {
        CommandLogManager clm=new CommandLogManager();
        UserManager um = new UserManager();
        DataManager dm = new DataManager();
        StatusManager sm = new StatusManager();
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
                    StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "html/log-timeline.html");
                    string html = reader.ReadToEnd();
                    reader.Close();
                    context.Response.Write(html);
                }
            }
            else
            {
                int count = 0;
                NodeCommand u = new NodeCommand();
                if ((count = clm.GetRecordCount(u)) > 0)
                {
                    List<NodeCommand> list = clm.getModelListALL(u);
                    Simple_Result2 sd = new Simple_Result2();
                    sd.result = "true";
                    sd.count = count / 10 + 1;
                    sd.data = create_log_list(list, int.Parse(context.Request["index"]));
                    list.Clear();
                    context.Response.Write(JsonConvert.SerializeObject(sd));
                }
                else
                    context.Response.Write("false");
            }
        }
        private string create_log_list(List<NodeCommand> list,int index)
        {
            StringBuilder sb = new StringBuilder();
            int i;
            for (i = 0; i < index && i < list.Count; i++)
            {
                NodeStatus nd = new NodeStatus();
                nd.Addr = list[i].Addr.Substring(2, 2) + list[i].Addr.Substring(0, 2);
                nd = sm.GetListByPage(nd);
                if (nd != null)
                {
                    sb.Append("<div class='tl-post'>");
                    sb.Append("<span class='icon'>&#59160;</span>");
                    string act = null;
                    switch (nd.Modle)
                    {
                        case "01":
                            if (list[i].Data == "0")
                                act = "关闭主卧灯！";
                            else
                                act = "打开主卧灯！";
                            break;
                        case "02":
                            if (list[i].Data == "0")
                                act = "关闭次卧灯！";
                            else
                                act = "打开次卧灯！";
                            break;
                        case "03":
                            if (list[i].Data == "0")
                                act = "关闭大厅灯！";
                            else
                                act = "打开大厅灯！";
                            break;
                        case "04":
                            if (list[i].Data == "0")
                                act = "关闭楼梯灯！";
                            else
                                act = "打开楼梯灯！";
                            break;
                        case "05":
                            if (list[i].Data == "0")
                                act = "关闭厨房灯！";
                            else
                                act = "打开厨房灯！";
                            break;
                        case "06":
                            if (list[i].Data == "0")
                                act = "关闭书房灯！";
                            else
                                act = "打开书房灯！";
                            break;
                        case "07":
                            if (list[i].Data == "0")
                                act = "关闭车门！";
                            else
                                act = "打开车门！";
                            break;
                        case "08":
                            act = "操作了遥控器";
                            break;
                        case "09":
                            if (list[i].Data == "0")
                                act = "关闭窗帘！";
                            else
                                act = "打开窗帘！";
                            break;
                        case "0a":
                            if (list[i].Data == "0")
                                act = "关闭报警！";
                            else
                                act = "打开报警！";
                            break;
                        case "0c":
                            if (list[i].Data == "0")
                                act = "关闭报警！";
                            else
                                act = "打开报警！";
                            break;
                        default:
                            break;
                    }
                    User u = new User();
                    u.Id = int.Parse(list[i].User);
                    um.getModel(u);
                    sb.Append("<p><a href='#'>" + u.Name + "</a>" + act + "<span class='time'>" + list[i].Time.ToString() + "</span></p>");
                    sb.Append("</div>");
                }
            }
            return sb.ToString();
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}