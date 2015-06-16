using cn.com.farsight.SH.SHDbVisit;
using cn.com.farsight.SH.SHModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace cn.com.farsight.SH.SHWebApplication.Admin
{
    /// <summary>
    /// users 的摘要说明
    /// </summary>
    public class users : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            UserManager um = new UserManager();
            if (context.Request.RequestType == "GET")
            {
                context.Response.ContentType = "text/html";
                if (context.Session["LoginUserName"] == null || string.IsNullOrEmpty(context.Session["LoginUserName"].ToString()))
                    context.Response.Write("<script>alert(\"权限不足,请重新登录!\");window.location.href=\"login.ashx\"</script>");
                else
                {

                    context.Response.ContentType = "text/html";
                    StreamReader reader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "html/users.html");
                    string html = reader.ReadToEnd();
                    reader.Close();
                    context.Response.Write(html);
                }
            }
            else
            {
                if (context.Request["type"] == "getusers")
                {
                    int count = 0;
                    User u = new User();
                    if ((count = um.GetRecordCount(u)) > 0)
                    {
                        List<User> list = um.GetListByPage(u);
                        Simple_Result2 sd = new Simple_Result2();
                        if ("1".Equals(context.Session["LoginUserPermisson"].ToString()))
                        {
                            sd.result = "true";
                            sd.data = create_user_list(list);
                        }
                        else
                        {
                            sd.result = "false";
                            sd.data = "用户没有权限管理用户信息！";
                        }
                        sd.count = count;
                        list.Clear();
                        context.Response.Write(JsonConvert.SerializeObject(sd));


                    }
                    else
                        context.Response.Write("false");
                }
                else if (context.Request["type"] == "adduser")
                {
                    User u = new User();
                    Simple_Result lresult = new Simple_Result();
                    u.Name = context.Request["name"];
                    if (um.getModel(u))
                    {
                        lresult.result = "have";
                        lresult.data = "用户名已存在！";
                        context.Response.Write(JsonConvert.SerializeObject(lresult));
                    }
                    else
                    {
                        u.User_permisson = 0;
                        u.Pwd = context.Request["pwd"];
                        u.Time = DateTime.Now;
                        if (um.addModel(u))
                        {
                            lresult.result = "true";
                            lresult.data = "../Admin/users.ashx";
                            context.Response.Write(JsonConvert.SerializeObject(lresult));
                        }
                        else
                        {
                            lresult.result = "false";
                            lresult.data = "添加用户失败！请重试！";
                            context.Response.Write(JsonConvert.SerializeObject(lresult));
                        }
                    }
                }
                else if (context.Request["type"] == "deluser")
                {
                    User u = new User();
                    u.Id = int.Parse(context.Request["id"]);
                    if(um.deleteModel(u))
                        context.Response.Write("true");
                    else
                        context.Response.Write("false");
                }
                else if (context.Request["type"] == "GetUserName")
                {
                    Simple_Result lresult = new Simple_Result();
                    if (context.Session["LoginUserName"] == null || string.IsNullOrEmpty(context.Session["LoginUserName"].ToString()))
                    {
                        lresult.result = "false";
                        lresult.data = "用户权限不足,请重新登录!";
                        context.Response.Write(JsonConvert.SerializeObject(lresult));
                        
                    }
                    else
                    {
                        lresult.result = "true";
                        lresult.data = context.Session["LoginUserName"].ToString();
                        context.Response.Write(JsonConvert.SerializeObject(lresult));
                    }
                }
                else if (context.Request["type"] == "Jurisdiction")
                {
                    if (context.Session["LoginUserName"] == null || string.IsNullOrEmpty(context.Session["LoginUserName"].ToString()))
                    {
                        context.Response.Write("false");
                    }
                    else
                    {
                        context.Response.Write("true");
                    }
                }
            }
        }
        private string create_user_list(List<User> list)
        {
            StringBuilder sb = new StringBuilder();
            int i;
            sb.Append("<thead><tr><th class='avatar'>用户名</th><th>权限</th><th>密码</th><th>最后登录时间</th><th>操作</th></tr></thead><tbody>");
            for (i = 0; i < 10 && i < list.Count; i++)
            {
                sb.Append("<tr><td class='avatar'><img src='../images/uiface1.png' alt='' height='40' width='40' />" + list[i].Name +
                    "</td><td>" + list[i].User_permisson.ToString() +
                    "</td><td>" + list[i].Pwd +
                    "</td><td>" + list[i].Time +
                    "</td><td><span class='delete' onclick = 'delete_user(" + list[i].Id.Value.ToString() + ")'>删除</span></td></tr>");
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