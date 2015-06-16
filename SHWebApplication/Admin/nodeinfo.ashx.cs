using cn.com.farsight.SH.SHDbVisit;
using cn.com.farsight.SH.SHModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace cn.com.farsight.SH.SHWebApplication.Admin
{
    /// <summary>
    /// nodeinfo 的摘要说明
    /// </summary>
    public class nodeinfo : IHttpHandler, IRequiresSessionState
    {
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
                StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "html/pages-table.html");
                string html = reader.ReadToEnd();
                reader.Close();
                context.Response.Write(html);
                }
            }
            else
            {
                int count = 0;
                NodeStatus u = new NodeStatus();
                if ((count = sm.GetRecordCount(u)) > 0)
                {
                    List<NodeStatus> list = sm.getModelListAll(u);
                    Simple_Result2 sd = new Simple_Result2();
                    sd.result = "true";
                    sd.count = count / 10 + 1;
                    sd.data = create_node_list(list);
                    list.Clear();
                    context.Response.Write(JsonConvert.SerializeObject(sd));
                }
                else
                    context.Response.Write("false");
            }
        }
        private string create_node_list(List<NodeStatus> list)
        {
            StringBuilder sb = new StringBuilder();
            int i;
            sb.Append("<thead><tr><th>节点</th><th>值</th><th>状态</th><th>时间</th><th>备注</th></tr></thead><tbody>");
            for (i = 0; i < 11 && i < list.Count; i++)
            {
                int b = Convert.ToInt32(list[i].Modle, 16); 
                if (b > 0 && b < 12)
                {
                    switch (b)
                    {
                        case 1:
                            sb.Append("<tr><td>" + "主卧灯");
                            if (list[i].Data == "000001")
                                sb.Append("</td><td>" + "开");
                            else
                                sb.Append("</td><td>" + "关");
                            break;
                        case 2:
                            sb.Append("<tr><td>" + "次卧灯");
                            if (list[i].Data == "000001")
                                sb.Append("</td><td>" + "开");
                            else
                                sb.Append("</td><td>" + "关");
                            break;
                        case 3:
                            sb.Append("<tr><td>" + "大厅灯");
                            if (list[i].Data == "000001")
                                sb.Append("</td><td>" + "开");
                            else
                                sb.Append("</td><td>" + "关");
                            break;
                        case 4:
                            sb.Append("<tr><td>" + "楼梯灯");
                            if (list[i].Data == "000001")
                                sb.Append("</td><td>" + "开");
                            else
                                sb.Append("</td><td>" + "关");
                            break;
                        case 5:
                            sb.Append("<tr><td>" + "厨房灯");
                            if (list[i].Data == "000001")
                                sb.Append("</td><td>" + "开");
                            else
                                sb.Append("</td><td>" + "关");
                            break;
                        case 6:
                            sb.Append("<tr><td>" + "书房灯");
                            if (list[i].Data == "000001")
                                sb.Append("</td><td>" + "开");
                            else
                                sb.Append("</td><td>" + "关");
                            break;
                        case 7:
                            sb.Append("<tr><td>" + "车库门");
                            if (list[i].Data == "000001")
                                sb.Append("</td><td>" + "开");
                            else
                                sb.Append("</td><td>" + "关");
                            break;
                        case 8:
                            sb.Append("<tr><td>" + "遥控器");
                            sb.Append("</td><td>");
                            break;
                        case 9:
                            sb.Append("<tr><td>" + "窗帘");
                            if (list[i].Data == "000001")
                                sb.Append("</td><td>" + "开");
                            else
                                sb.Append("</td><td>" + "关");
                            break;
                        case 10:
                            sb.Append("<tr><td>" + "报警");
                            if (list[i].Data == "000001")
                                sb.Append("</td><td>" + "开");
                            else
                                sb.Append("</td><td>" + "关");
                            break;
                        case 11:
                            sb.Append("<tr><td>" + "检测杠");
                            if (list[i].Data[0] == '1')
                                sb.Append("</td><td>" + "开");
                            else
                                sb.Append("</td><td>" + "关");
                            if (list[i].Time.CompareTo(DateTime.Now.AddSeconds(-30)) > 0)
                            {
                                sb.Append("</td><td>" + "online");
                            }
                            else
                                sb.Append("</td><td style='color:#ff0000'>" + "offline");
                            sb.Append("</td><td>" + list[i].Time);
                            sb.Append("</td><td>" +
                             "</td></tr>");

                            sb.Append("<tr><td>" + "门磁");
                            if (list[i].Data[1] == '1')
                                sb.Append("</td><td>" + "开");
                            else
                                sb.Append("</td><td>" + "关");
                            if (list[i].Time.CompareTo(DateTime.Now.AddSeconds(-30)) > 0)
                            {
                                sb.Append("</td><td>" + "online");
                            }
                            else
                                sb.Append("</td><td style='color:#ff0000'>" + "offline");
                            sb.Append("</td><td>" + list[i].Time);
                            sb.Append("</td><td>" +
                             "</td></tr>");

                            sb.Append("<tr><td>" + "温度");
                            sb.Append("</td><td>" + int.Parse(list[i].Data.Substring(2, 2), NumberStyles.HexNumber) + "℃");
                            if (list[i].Time.CompareTo(DateTime.Now.AddSeconds(-30)) > 0)
                            {
                                sb.Append("</td><td>" + "online");
                            }
                            else
                                sb.Append("</td><td style='color:#ff0000'>" + "offline");
                            sb.Append("</td><td>" + list[i].Time);
                            sb.Append("</td><td>" +
                             "</td></tr>");

                            sb.Append("<tr><td>" + "湿度");
                            sb.Append("</td><td>" + int.Parse(list[i].Data.Substring(4, 2), NumberStyles.HexNumber)+"%");
                            break;
                    }
                    if (list[i].Time.CompareTo(DateTime.Now.AddSeconds(-10)) > 0)
                    {
                        sb.Append("</td><td>" + "online");
                    }
                    else
                        sb.Append("</td><td style='color:#ff0000'>" + "offline");
                    sb.Append("</td><td>" + list[i].Time);
                    sb.Append("</td><td>" +
                     "</td></tr>");
                }
            }
            sb.Append("</tbody>");
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